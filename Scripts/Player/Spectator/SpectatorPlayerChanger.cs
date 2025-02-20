using Godot;
using System;
using System.Collections.Generic;

public partial class SpectatorPlayerChanger : Node
{
    [Export] private InputActions _inputActions;

    private readonly List<Node> _playerRootList = new();
    private int _spectatingPlayerNumber;

    public Action<string> SpectatedPlayerChanged;

    public override void _Ready()
    {
        _inputActions.SpectateNextPlayerDown += NextPlayer;
        _inputActions.SpectatePreviousPlayerDown += PreviousPlayer;

        UpdatePlayersNode();
        if (_playerRootList.Count > 0)
            InvokePlayerChange(_spectatingPlayerNumber);
    }

    public override void _ExitTree()
    {
        _inputActions.SpectateNextPlayerDown -= NextPlayer;
        _inputActions.SpectatePreviousPlayerDown -= PreviousPlayer;
    }

    private void NextPlayer()
    {
        UpdatePlayersNode();

        _spectatingPlayerNumber = _spectatingPlayerNumber + 1 < _playerRootList.Count
            ? _spectatingPlayerNumber + 1
            : 0;
        InvokePlayerChange(_spectatingPlayerNumber);
    }

    private void PreviousPlayer()
    {
        UpdatePlayersNode();

        _spectatingPlayerNumber = _spectatingPlayerNumber - 1 >= 0
            ? _spectatingPlayerNumber - 1
            : _playerRootList.Count - 1;

        InvokePlayerChange(_spectatingPlayerNumber);
    }

    private void UpdatePlayersNode()
    {
        _playerRootList.Clear();
        foreach (Node playerNode in GetTree().GetNodesInGroup("Player:Human"))
            _playerRootList.Add(playerNode);
    }

    private void InvokePlayerChange(int playerNumber)
    {
        Node playerRoot = _playerRootList[playerNumber];
        SpectatedPlayerChanged?.Invoke(playerRoot.GetPath());
    }
}
