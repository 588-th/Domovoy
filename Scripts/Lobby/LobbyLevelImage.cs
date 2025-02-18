using Godot;
using System;

public partial class LobbyLevelImage : Node
{
	[Export] private Sprite3D _levelImage;

	[Export] private Texture2D _apartmentImage;
	[Export] private Texture2D _bunkerImage;

	public override void _Ready()
	{
		SettingsRound.Instance.MapUpdated += UpdateImage;
    }

    public override void _ExitTree()
    {
        SettingsRound.Instance.MapUpdated -= UpdateImage;
    }

    private void UpdateImage(PackedScene mapScene)
	{
        switch (mapScene.ResourcePath)
        {
            case "res://Scenes/Levels/LevelApartments.tscn":
                {
                    GlobalRpcFunctions.Instance.SetTextureToSprite3D(_levelImage.GetPath(), _apartmentImage.ResourcePath);
                    break;
                }
            case "res://Scenes/Levels/LevelBunker.tscn":
                {
                    GlobalRpcFunctions.Instance.SetTextureToSprite3D(_levelImage.GetPath(), _bunkerImage.ResourcePath);
                    break;
                }
        }
    }
}
