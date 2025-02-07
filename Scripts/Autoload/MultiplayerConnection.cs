using Godot;
using System;

public partial class MultiplayerConnection : Node
{
    public static MultiplayerConnection Instance { get; private set; }

    public enum EnumConnectionState { None, Server, Client }
    public EnumConnectionState ConnectionState = EnumConnectionState.None;

    public Action ServerCreated;
    public Action ServerClosed;
    public Action ClientCreated;
    public Action ClientClosed;
    public Action ClientCreateFailed;

    public override void _Ready()
    {
        Instance = this;
    }

    public void CreateServer(int port)
    {
        if (ConnectionState != EnumConnectionState.None)
            return;

        ENetMultiplayerPeer peer = new();
        peer.CreateServer(port);

        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
            return;

        Multiplayer.MultiplayerPeer = peer;
        ConnectionState = EnumConnectionState.Server;
        ServerCreated?.Invoke();
    }

    public void CloseServer()
    {
        if (ConnectionState != EnumConnectionState.Server)
            return;

        foreach (var peerID in Multiplayer.GetPeers())
            RpcId(peerID, nameof(RpcCloseClient));

        if (Multiplayer.GetPeers().Length == 0)
        {
            Multiplayer.MultiplayerPeer.Dispose();
            Multiplayer.MultiplayerPeer = null;
            ConnectionState = EnumConnectionState.None;
            ServerClosed?.Invoke();
        }
        else
            Multiplayer.MultiplayerPeer.PeerDisconnected += OnAllPeerDisconnected;
    }

    public void CreateClient(string ip, int port)
    {
        if (ConnectionState != EnumConnectionState.None)
            return;

        ENetMultiplayerPeer peer = new();
        peer.CreateClient(ip, port);

        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
        {
            ClientCreateFailed.Invoke();
            return;
        }

        Multiplayer.ServerDisconnected += CloseClient;

        ConnectionState = EnumConnectionState.Client;
        Multiplayer.MultiplayerPeer = peer;
        ClientCreated?.Invoke();
    }

    public void CloseClient()
    {
        if (ConnectionState != EnumConnectionState.Client)
            return;

        if (Multiplayer.MultiplayerPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connecting)
            Multiplayer.MultiplayerPeer.DisconnectPeer(Multiplayer.GetUniqueId());

        Multiplayer.ServerDisconnected -= CloseClient;

        Multiplayer.MultiplayerPeer.Dispose();
        Multiplayer.MultiplayerPeer = null;
        ConnectionState = EnumConnectionState.None;
        ClientClosed?.Invoke();
    }

    private void OnAllPeerDisconnected(long peer)
    {
        if (Multiplayer.GetPeers().Length == 0)
        {
            Multiplayer.MultiplayerPeer.PeerDisconnected -= OnAllPeerDisconnected;
            Multiplayer.MultiplayerPeer.Dispose();
            Multiplayer.MultiplayerPeer = null;
            ConnectionState = EnumConnectionState.None;
            ServerClosed?.Invoke();
        }
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RpcCloseClient()
    {
        CloseClient();
    }
}
