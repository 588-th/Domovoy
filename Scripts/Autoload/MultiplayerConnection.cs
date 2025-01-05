using Godot;
using System;

public partial class MultiplayerConnection : Node
{
    public static MultiplayerConnection Instance { get; private set; }

    private enum ConnectionStateEnum { None, Server, Client }
    private ConnectionStateEnum connectionState = ConnectionStateEnum.None;

    public Action ServerCreated;
    public Action ServerClosed;
    public Action ClientCreated;
    public Action ClientClosed;

    public override void _Ready()
    {
        Instance = this;
    }

    public void CreateServer(int port)
    {
        if (connectionState != ConnectionStateEnum.None)
            return;

        ENetMultiplayerPeer peer = new();
        peer.CreateServer(port);
        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
            return;

        Multiplayer.MultiplayerPeer = peer;
        connectionState = ConnectionStateEnum.Server;
        ServerCreated?.Invoke();
    }

    public void CreateClient(string ip, int port)
    {
        if (connectionState != ConnectionStateEnum.None)
            return;

        ENetMultiplayerPeer peer = new();
        peer.CreateClient(ip, port);

        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
            return;

        Multiplayer.ServerDisconnected += CloseClient;

        connectionState = ConnectionStateEnum.Client;
        Multiplayer.MultiplayerPeer = peer;
        ClientCreated?.Invoke();
    }

    public void CloseServer()
    {
        if (connectionState != ConnectionStateEnum.Server)
            return;

        foreach (var peerID in Multiplayer.GetPeers())
            RpcId(peerID, nameof(RpcCloseClient));

        Multiplayer.MultiplayerPeer.PeerDisconnected += OnPeerDisconnected;
    }

    private void OnPeerDisconnected(long peerID)
    {
        if (Multiplayer.GetPeers().Length == 0)
        {
            Multiplayer.MultiplayerPeer.PeerDisconnected -= OnPeerDisconnected;
            Multiplayer.MultiplayerPeer.Dispose();
            Multiplayer.MultiplayerPeer = null;
            connectionState = ConnectionStateEnum.None;
            ServerClosed?.Invoke();
        }
    }

    public void CloseClient()
    {
        if (connectionState != ConnectionStateEnum.Client)
            return;

        if (Multiplayer.MultiplayerPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connecting)
            Multiplayer.MultiplayerPeer.DisconnectPeer(Multiplayer.GetUniqueId());

        Multiplayer.ServerDisconnected -= CloseClient;

        Multiplayer.MultiplayerPeer.Dispose();
        Multiplayer.MultiplayerPeer = null;
        connectionState = ConnectionStateEnum.None;
        ClientClosed?.Invoke();
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RpcCloseClient()
    {
        CloseClient();
    }
}
