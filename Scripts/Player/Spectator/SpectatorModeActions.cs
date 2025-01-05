using Godot;
using System;

public partial class SpectatorModeActions : Node
{
    public Action<string> SpectatedPlayerChanged;

    public void InvokeSpectatedPlayerChanged(string spectatedPlayerPath)
    {
        RpcId(1, nameof(RpcInvokeSpectatedPlayerChanged), spectatedPlayerPath);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RpcInvokeSpectatedPlayerChanged(string spectatedPlayerPath)
    {
        SpectatedPlayerChanged?.Invoke(spectatedPlayerPath);
    }
}
