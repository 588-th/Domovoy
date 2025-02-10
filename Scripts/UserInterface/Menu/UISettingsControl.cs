using Godot;
using System;
using System.Collections.Generic;

public partial class UISettingsControl : Control
{
    [Export] private Control _UIWindowSettings;
    [Export] private VBoxContainer _actionList;
    [Export] private Button _buttonClose;
    [Export] private PackedScene _actionLineScene;

    private bool _isRemapping;
    private string _currentRemapAction;
    private Label _currentRemapLabel;
    private Button _currentResetButton;

    private static readonly Dictionary<string, string> _inputActions = new()
    {
        { "up", "Forward" }, { "down", "Backward" }, { "left", "Left" }, { "right", "Right" },
        { "sneak", "Sneak" }, { "crouch", "Crouch" }, { "jump", "Jump" }, { "interact", "Interact" },
        { "drop", "Drop" }, { "attack", "Attack" }, { "alternative", "Alternative" },
        { "escape", "Escape" }, { "primarySlot", "Slot Primary" }, { "secondarySlot", "Slot Secondary" },
        { "tertiarySlot", "Slot Tertiary" }, { "quaternarySlot", "Slot Quaternary" },
        { "spectateNextPlayer", "Spectate Next Player" }, { "spectatPreviousPlayer", "Spectate Previous Player" },
        { "toggleMonsterVision", "Toggle Monster Vision" }, { "toggleFirearmLaser", "Toggle Firearm Laser" },
        { "toggleFlashlight", "Toggle Flashlight" }, { "toggleMicrophone", "Toggle Microphone" },
    };

    public override void _Ready()
    {
        _buttonClose.Pressed += () => _UIWindowSettings.Hide();
        CreateActionList();
    }

    private void CreateActionList()
    {
        foreach (Node action in _actionList.GetChildren())
            action.QueueFree();

        foreach (var (action, label) in _inputActions)
        {
            var actionLine = _actionLineScene.Instantiate<Control>();
            var actionLabel = actionLine.FindChild("LabelAction") as Label;
            var inputLabel = actionLine.FindChild("LabelInput") as Label;
            var actionButton = actionLine.FindChild("ButtonAction") as Button;
            var resetButton = actionLine.FindChild("ButtonReset") as Button;

            actionLabel.Text = label;
            inputLabel.Text = GetActionKeyText(action);

            actionButton.Pressed += () => StartRemap(inputLabel, resetButton, action);
            resetButton.Pressed += () => ResetKeybind(inputLabel, resetButton, action);
            resetButton.Disabled = IsDefaultKeybind(action);

            _actionList.AddChild(actionLine);
        }
    }

    private string GetActionKeyText(string action) =>
        SettingsControl.Instance.GetKeybind(action)?.AsText().TrimSuffix(" (Physical)") ?? string.Empty;

    private void StartRemap(Label inputLabel, Button resetButton, string action)
    {
        if (_isRemapping) return;

        _isRemapping = true;
        _currentRemapAction = action;
        _currentRemapLabel = inputLabel;
        _currentResetButton = resetButton;
        inputLabel.Text = "Press key to bind...";
    }

    public override void _Input(InputEvent @event)
    {
        if (!_isRemapping || !(@event is InputEventKey or InputEventMouseButton { Pressed: true })) return;

        SettingsControl.Instance.SetKeybind(_currentRemapAction, @event);
        _currentRemapLabel.Text = @event.AsText().TrimSuffix(" (Physical)");
        _currentResetButton.Disabled = IsDefaultKeybind(_currentRemapAction);

        _isRemapping = false;
        _currentRemapAction = null;
        _currentRemapLabel = null;
        _currentResetButton = null;

        AcceptEvent();
    }

    private void ResetKeybind(Label inputLabel, Button resetButton, string action)
    {
        SettingsControl.Instance.ResetKeybind(action);
        inputLabel.Text = GetActionKeyText(action);
        resetButton.Disabled = true;
    }

    private bool IsDefaultKeybind(string action)
    {
        var currentKey = SettingsControl.Instance.GetKeybind(action);
        var defaultKey = SettingsControl.Instance.GetDefaultKeybind(action);
        return currentKey.AsText() == defaultKey.AsText();
    }
}
