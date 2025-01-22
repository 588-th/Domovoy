using Godot;
using System;
using System.Collections.Generic;

public partial class MovementActions : Node
{
    [Export] private Node _playerRoot;

    public Action IsIdleState;
    public Action IsWalkState;
    public Action IsJumpState;
    public Action IsNotIdleState;
    public Action IsNotWalkState;
    public Action IsNotJumpState;
    public Action IsGrounded;
    public Action IsNotGrounded;

    private Dictionary<string, Func<Action>> _movementActions;

    public override void _Ready()
    {
        _movementActions = new()
        {
            {"isIdleState", () => IsIdleState},
            {"isWalkState", () => IsWalkState},
            {"isJumpState", () => IsJumpState},
            {"isNotIdleState", () => IsNotWalkState},
            {"isNotWalkState", () => IsNotWalkState},
            {"isNotJumpState", () => IsNotJumpState},
            {"isGrounded", () => IsGrounded},
            {"isNotGrounded", () => IsNotGrounded},
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
        if (_movementActions.ContainsKey(actionName))
        {
            Action action = _movementActions[actionName]();
            action?.Invoke();
        }
    }
}
