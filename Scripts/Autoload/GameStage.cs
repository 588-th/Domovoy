using Godot;
using System;

public partial class GameStage : Node
{
    public static GameStage Instance { get; private set; }

    public EnumGameStage CurrentGameStage = EnumGameStage.MainMenu;
    public Action MainMenuStage;
    public Action LobbyStage;
    public Action RoundStage;

    public enum EnumGameStage
    {
        MainMenu,
        Lobby,
        Round
    }

    public override void _Ready()
    {
        Instance = this;
    }

    public void ChangeStageToMainMenu()
    {
        CurrentGameStage = EnumGameStage.MainMenu;
        MainMenuStage?.Invoke();
    }

    public void ChangeStageToLobby()
    {
        CurrentGameStage = EnumGameStage.Lobby;
        LobbyStage?.Invoke();
    }

    public void ChangeStateToRound()
    {
        CurrentGameStage = EnumGameStage.Round;
        RoundStage?.Invoke();
    }
}
