using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeAttackActions : Node
{
    [Export] private Node _playerRoot;

    public Action CooldownStart;
    public Action CooldownEnd;
    public Action Attack;

    private Dictionary<string, Func<Action>> _attackActions;

    public override void _Ready()
    {
        _attackActions = new()
        {
            {"CooldownStart", () => CooldownStart},
            {"CooldownEnd", () => CooldownEnd},
            {"Attack", () => Attack},
        };
    }

    public void InvokeAction(string actionName)
    {
        RpcId(1, nameof(RpcInvokeAction), actionName);

        if (_playerRoot.Name != "1")
            RpcId(int.Parse(_playerRoot.Name), nameof(RpcInvokeAction), actionName);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RpcInvokeAction(string actionName)
    {
        if (_attackActions.ContainsKey(actionName))
        {
            Action action = _attackActions[actionName]();
            action?.Invoke();
        }
    }
}
