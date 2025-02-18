using Godot;
using System.Linq;

public partial class UISettingsRound : Node
{
    [Export] private Control _UIWindowSettings;
    [Export] private OptionButton _optionButtonMap;
    [Export] private Button _buttonClose;

    private SpawnSystemLevel _spawnSystemLevel;

    private bool _isSubsctive;

    public override void _Ready()
    {
        if (MultiplayerConnection.Instance.ConnectionState != MultiplayerConnection.EnumConnectionState.Server)
            return;

        if (GameStage.Instance.CurrentGameStage != GameStage.EnumGameStage.Lobby)
            return;

        _optionButtonMap.ItemSelected += OnMapSelected;
        _buttonClose.Pressed += OnCloseButtonPressed;
        _isSubsctive = true;

        InitializeUI();
    }

    public override void _ExitTree()
    {
        if (!_isSubsctive)
            return;

        _optionButtonMap.ItemSelected -= OnMapSelected;
        _buttonClose.Pressed -= OnCloseButtonPressed;
    }

    private void InitializeUI()
    {
        Node node = GetTree().GetFirstNodeInGroup("SpawnSystem:Level");
        _spawnSystemLevel = node as SpawnSystemLevel;

        if (_spawnSystemLevel.RoundLevelScene.Count == 0)
            return;

        for (int i = 0; i < _spawnSystemLevel.RoundLevelScene.Count; i++)
            _optionButtonMap.AddItem(_spawnSystemLevel.RoundLevelScene[i].ResourcePath, i);

        _optionButtonMap.Selected = 0;
        GD.Print("a");
        SettingsRound.Instance.Map = _spawnSystemLevel.RoundLevelScene[0];
    }

    private void OnMapSelected(long id)
    {
        if (id >= 0 && id < _spawnSystemLevel.RoundLevelScene.Count)
            SettingsRound.Instance.Map = _spawnSystemLevel.RoundLevelScene[(int)id];
    }

    private void OnCloseButtonPressed()
    {
        _UIWindowSettings.Hide();
    }
}
