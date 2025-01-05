using Godot;
using System;
using System.Collections.Generic;

public partial class SpectatorMode : Node
{
    [Export] private SpectatorInputActions _spectatorInputActions;
    [Export] private SpectatorModeActions _spectatorModeActions;
    private readonly List<Node> _playersNode = new();
    private int _spectatingPlayerNumber;

    public override void _Ready()
    {
        _spectatorInputActions.SpectateNextPlayerDown += NextPlayer;
        _spectatorInputActions.SpectatePreviousPlayerDown += PreviousPlayer;

        UpdatePlayersNode();
        SpectatePlayer(_spectatingPlayerNumber);
    }

    public override void _ExitTree()
    {
        _spectatorInputActions.SpectateNextPlayerDown -= NextPlayer;
        _spectatorInputActions.SpectatePreviousPlayerDown -= PreviousPlayer;
    }

    private void NextPlayer()
    {
        UpdatePlayersNode();
        int playerNumber = _spectatingPlayerNumber + 1 >= _playersNode.Count ? _spectatingPlayerNumber + 1 : 0;
        SpectatePlayer(playerNumber);
    }

    private void PreviousPlayer()
    {
        UpdatePlayersNode();
        int playerNumber = _spectatingPlayerNumber - 1 >= 0 ? _spectatingPlayerNumber - 1 : _playersNode.Count - 1;
        SpectatePlayer(playerNumber);
    }

    private void UpdatePlayersNode()
    {
        _playersNode.Clear();
        foreach (Node playerNode in GetTree().GetNodesInGroup("Human"))
        {
            GD.Print("PlayerNode: " + playerNode);
            _playersNode.Add(playerNode);
        }
    }

    private void SpectatePlayer(int playerNumber)
    {
        Node playerRoot = _playersNode[playerNumber];

        var playerCameraPath = playerRoot.GetMeta("PlayerCamera");
        Camera3D playerCamera = playerRoot.GetNode(playerCameraPath.ToString()) as Camera3D;
        playerCamera.Current = true;

        _spectatorModeActions.InvokeSpectatedPlayerChanged(playerRoot.GetPath());
    }
}
