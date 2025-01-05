using Godot;

public partial class HUDParameterSpectating : Node
{
    [Export] private HUDParameters _playerData;
    [Export] private SpectatorModeActions _spectatorModeEvents;

    private HUDParameters _spectatingPlayerData;

    public override void _Ready()
    {
        _spectatorModeEvents.SpectatedPlayerChanged += SpectatePlayerData;
    }

    public override void _ExitTree()
    {
        UnspectatePlayerData();
        _spectatorModeEvents.SpectatedPlayerChanged -= SpectatePlayerData;
    }

    private void SpectatePlayerData(string playerPath)
    {
        Node playerRoot = GetNode(playerPath);

        UnspectatePlayerData();

        HUDParameters playerData = playerRoot.GetNode<HUDParameters>("ClientServerPart/Scripts/HUDParameters");
        _spectatingPlayerData = playerData;

        playerData.PlayerHealthUpdated += TransferHealthData;
        playerData.PlayerHealthMaxUpdated += TransferMaxHealthData;
        playerData.PlayerActiveSlotItemUpdated += TransferItemData;
        playerData.PlayerInteractDataUpdated += TransferInteractData;

        TransferHealthData();
        TransferMaxHealthData();
        TransferItemData();
        TransferInteractData();
    }

    private void UnspectatePlayerData()
    {
        if (_spectatingPlayerData == null)
            return;

        _spectatingPlayerData.PlayerHealthUpdated -= TransferHealthData;
        _spectatingPlayerData.PlayerHealthMaxUpdated -= TransferMaxHealthData;
        _spectatingPlayerData.PlayerActiveSlotItemUpdated -= TransferItemData;
        _spectatingPlayerData.PlayerInteractDataUpdated -= TransferInteractData;
    }

    private void TransferHealthData()
    {
        _playerData.PlayerHealth = _spectatingPlayerData.PlayerHealth;
    }

    private void TransferMaxHealthData()
    {
        _playerData.PlayerHealthMax = _spectatingPlayerData.PlayerHealthMax;
    }

    private void TransferItemData()
    {
        _playerData.PlayerActiveSlotItem = _spectatingPlayerData.PlayerActiveSlotItem;
    }

    private void TransferInteractData()
    {
        _playerData.PlayerInteractData = _spectatingPlayerData.PlayerInteractData;
    }
}
