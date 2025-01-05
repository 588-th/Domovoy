using Godot;
using System;

public partial class GameState : Node
{
    public static GameState Instance { get; private set; }

    public GameStateEnum CurrentGameState = GameStateEnum.None;
    public Action LobbyStarted;
    public Action RoundStarted;

    public enum GameStateEnum
    {
        None,
        Lobby,
        Round
    }

    public override void _Ready()
    {
        Instance = this;
    }

    public void StartLobby()
    {
        CurrentGameState = GameStateEnum.Lobby;
        LobbyStarted?.Invoke();
    }

    public void StartRound()
    {
        CurrentGameState = GameStateEnum.Round;
        RoundStarted?.Invoke();
    }
}
