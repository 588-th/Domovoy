using Godot;

public partial class HUDParameterSpectating : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private SpectatorPlayerChanger _spectatorPlayerChanger;

    private HUDParameters _spectatingPlayerParameters;

    public override void _Ready()
    {
        _spectatorPlayerChanger.SpectatedPlayerChanged += SpectatePlayerData;
    }

    public override void _ExitTree()
    {
        Unsubscribe();
        _spectatorPlayerChanger.SpectatedPlayerChanged -= SpectatePlayerData;
    }

    private void SpectatePlayerData(string playerPath)
    {
        Node playerRoot = GetNode(playerPath);

        Unsubscribe();

        HUDParameters playerData = playerRoot.GetNode<HUDParameters>("ClientServerPart/Scripts/HUDParameters");
        _spectatingPlayerParameters = playerData;

        Subscribe();

        TransferNameData(playerRoot.Name);
        TransferHealthData();
        TransferMaxHealthData();
        TransferItemData();
        TransferInteractData();
    }

    private void Subscribe()
    {
        _spectatingPlayerParameters.PlayerHealthUpdated += TransferHealthData;
        _spectatingPlayerParameters.PlayerHealthMaxUpdated += TransferMaxHealthData;
        _spectatingPlayerParameters.PlayerHoldingItemUpdated += TransferItemData;
        _spectatingPlayerParameters.PlayerInteractDataUpdated += TransferInteractData;
    }

    private void Unsubscribe()
    {
        if (_spectatingPlayerParameters == null)
            return;

        _spectatingPlayerParameters.PlayerHealthUpdated -= TransferHealthData;
        _spectatingPlayerParameters.PlayerHealthMaxUpdated -= TransferMaxHealthData;
        _spectatingPlayerParameters.PlayerHoldingItemUpdated -= TransferItemData;
        _spectatingPlayerParameters.PlayerInteractDataUpdated -= TransferInteractData;
    }

    private void TransferNameData(string name)
    {
        _hudParameters.PlayerName = name;
    }

    private void TransferHealthData()
    {
        _hudParameters.PlayerHealth = _spectatingPlayerParameters.PlayerHealth;
    }

    private void TransferMaxHealthData()
    {
        _hudParameters.PlayerHealthMax = _spectatingPlayerParameters.PlayerHealthMax;
    }

    private void TransferItemData()
    {
        _hudParameters.PlayerHoldingItem = _spectatingPlayerParameters.PlayerHoldingItem;
    }

    private void TransferInteractData()
    {
        _hudParameters.PlayerInteract = _spectatingPlayerParameters.PlayerInteract;
    }
}
