using Godot;
using System.Collections.Generic;

public partial class SettingsControl : Node
{
    public static SettingsControl Instance { get; private set; }

    private readonly Dictionary<string, InputEvent> _customKeybinds = new();
    private readonly Dictionary<string, InputEvent> _defaultKeybinds = new()
    {
        { "up", new InputEventKey { Keycode = Key.W } },
        { "down", new InputEventKey { Keycode = Key.S } },
        { "left", new InputEventKey { Keycode = Key.A } },
        { "right", new InputEventKey { Keycode = Key.D } },
        { "sneak", new InputEventKey { Keycode = Key.Shift } },
        { "crouch", new InputEventKey { Keycode = Key.Ctrl } },
        { "jump", new InputEventKey { Keycode = Key.Space } },
        { "drop", new InputEventKey { Keycode = Key.G } },
        { "interact", new InputEventKey { Keycode = Key.E } },
        { "primarySlot", new InputEventKey { Keycode = Key.Key1 } },
        { "secondarySlot", new InputEventKey { Keycode = Key.Key2 } },
        { "tertiarySlot", new InputEventKey { Keycode = Key.Key3 } },
        { "quaternarySlot", new InputEventKey { Keycode = Key.Key4 } },
        { "spectateNextPlayer", new InputEventMouseButton { ButtonIndex = MouseButton.Right } },
        { "spectatPreviousPlayer", new InputEventMouseButton { ButtonIndex = MouseButton.Left } },
        { "toggleMonsterVision", new InputEventKey { Keycode = Key.Capslock } },
        { "toggleFirearmLaser", new InputEventKey { Keycode = Key.C } },
        { "toggleFlashlight", new InputEventKey { Keycode = Key.F } },
        { "toggleMicrophone", new InputEventKey { Keycode = Key.V } },
        { "attack", new InputEventMouseButton { ButtonIndex = MouseButton.Left } },
        { "alternative", new InputEventMouseButton { ButtonIndex = MouseButton.Right } },
        { "escape", new InputEventKey { Keycode = Key.Escape } },
    };

    public override void _Ready() => Instance = this;

    public void SetKeybind(string action, InputEvent inputEvent)
    {
        _customKeybinds[action] = inputEvent;
        InputMap.ActionEraseEvents(action);
        InputMap.ActionAddEvent(action, inputEvent);
    }

    public InputEvent GetKeybind(string action) =>
        _customKeybinds.TryGetValue(action, out var customEvent) ? customEvent : _defaultKeybinds[action];

    public InputEvent GetDefaultKeybind(string action) => _defaultKeybinds[action];

    public void ResetKeybind(string action)
    {
        if (!_defaultKeybinds.ContainsKey(action)) return;

        SetKeybind(action, _defaultKeybinds[action]);
        _customKeybinds.Remove(action);
    }
}