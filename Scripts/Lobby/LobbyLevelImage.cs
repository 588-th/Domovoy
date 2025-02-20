using Godot;

public partial class LobbyLevelImage : Node
{
    [Export] private Sprite3D _levelImage;

    [Export] private Texture2D _apartmentImage;
    [Export] private Texture2D _bunkerImage;
    private Texture2D _currentLevelImage;

    public override void _Ready()
    {
        SettingsRound.Instance.MapUpdated += OnMapSelected;
        Multiplayer.PeerConnected += UpdateImage;
    }

    public override void _ExitTree()
    {
        SettingsRound.Instance.MapUpdated -= OnMapSelected;
        Multiplayer.PeerConnected -= UpdateImage;
    }

    private void OnMapSelected(PackedScene mapScene)
    {
        GetLevelImage(mapScene);
        UpdateImage(0);
    }

    private void GetLevelImage(PackedScene mapScene)
    {
        switch (mapScene.ResourcePath)
        {
            case "res://Scenes/Levels/LevelApartments.tscn":
                {
                    _currentLevelImage = _apartmentImage;
                    break;
                }
            case "res://Scenes/Levels/LevelBunker.tscn":
                {
                    _currentLevelImage = _bunkerImage;
                    break;
                }
        }
    }

    private void UpdateImage(long id)
    {
        GlobalRpcFunctions.Instance.SetTextureToSprite3D(_levelImage.GetPath(), _currentLevelImage.ResourcePath);
    }
}
