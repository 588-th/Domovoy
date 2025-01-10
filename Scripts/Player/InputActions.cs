using Godot;
using System;
using System.Collections.Generic;

public partial class InputActions : Node
{
    public Action EscapeKeyDown;
    public Action UpKeyDown;
    public Action DownKeyDown;
    public Action LeftKeyDown;
    public Action RightKeyDown;
    public Action CrouchKeyDown;
    public Action JumpKeyDown;
    public Action InteractKeyDown;
    public Action DropKeyDown;
    public Action AttackKeyDown;
    public Action AlternativeKeyDown;
    public Action ReloadKeyDown;
    public Action PrimarySlotDown;
    public Action SecondarySlotDown;
    public Action TertiarySlotDown;
    public Action ToggleMonsterVisionDown;
    public Action ToggleFirearmAutomaticDown;
    public Action ToggleFirearmLaserDown;
    public Action ToggleFlashlightDown;
    public Action SpectateNextPlayerDown;
    public Action SpectatePreviousPlayerDown;

    public Action EscapeKeyUp;
    public Action UpKeyUp;
    public Action DownKeyUp;
    public Action LeftKeyUp;
    public Action RightKeyUp;
    public Action CrouchKeyUp;
    public Action JumpKeyUp;
    public Action InteractKeyUp;
    public Action DropKeyUp;
    public Action AttackKeyUp;
    public Action AlternativeKeyUp;

    private Dictionary<string, Func<Action>> _keyDownActions;
    private Dictionary<string, Func<Action>> _keyUpActions;

    public override void _Ready()
    {
        _keyDownActions = new()
        {
            {"escape", () => EscapeKeyDown},
            {"up", () => UpKeyDown},
            {"down", () => DownKeyDown},
            {"left", () => LeftKeyDown},
            {"right", () => RightKeyDown},
            {"crouch", () => CrouchKeyDown},
            {"jump", () => JumpKeyDown},
            {"interact", () => InteractKeyDown},
            {"drop", () => DropKeyDown},
            {"attack", () => AttackKeyDown},
            {"alternative", () => AlternativeKeyDown},
            {"reload", () => ReloadKeyDown},
            {"primarySlot", () => PrimarySlotDown},
            {"secondarySlot", () => SecondarySlotDown},
            {"tertiarySlot", () => TertiarySlotDown},
            {"toggleMonsterVision", () => ToggleMonsterVisionDown},
            {"toggleFirearmAutomatic", () => ToggleFirearmAutomaticDown},
            {"toggleFirearmLaser", () => ToggleFirearmLaserDown},
            {"toggleFlashlight", () => ToggleFlashlightDown},
            {"spectateNextPlayer", () => SpectateNextPlayerDown},
            {"spectatePreviousPlayer", () => SpectatePreviousPlayerDown},
        };

        _keyUpActions = new()
        {
            {"escape", () => EscapeKeyUp},
            {"up", () => UpKeyUp},
            {"down", () => DownKeyUp},
            {"left", () => LeftKeyUp},
            {"right", () => RightKeyUp},
            {"crouch", () => CrouchKeyUp},
            {"jump", () => JumpKeyUp},
            {"interact", () => InteractKeyUp},
            {"drop", () => DropKeyUp},
            {"attack", () => AttackKeyUp},
            {"alternative", () => AlternativeKeyUp}
        };
    }

    public void InvokeAction(string actionName, bool isPressed)
    {
        RpcId(1, nameof(RpcInvokeAction), actionName, isPressed);

        if (Multiplayer.GetUniqueId() != 1)
            RpcId(Multiplayer.GetUniqueId(), nameof(RpcInvokeAction), actionName, isPressed);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RpcInvokeAction(string actionName, bool isPressed)
    {
        if (isPressed && _keyDownActions.ContainsKey(actionName))
        {
            Action action = _keyDownActions[actionName]();
            action?.Invoke();
        }
        else if (!isPressed && _keyUpActions.ContainsKey(actionName))
        {
            Action action = _keyUpActions[actionName]();
            action?.Invoke();
        }
    }
}
