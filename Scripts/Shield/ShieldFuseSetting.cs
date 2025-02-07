using Godot;
using Godot.Collections;

public partial class ShieldFuseSetting : Node
{
    [Export] private Array<Node3D> _fuses;

    private int _maxFuses;
    private int _currentFuses;

    public override void _Ready()
    {
        _maxFuses = _fuses.Count;
    }

    public void SettingFuse()
    {
        if (_currentFuses == _maxFuses)
            return;

        GlobalRpcFunctions.Instance.SetVisibility(_fuses[_currentFuses].GetPath(), true);
        _currentFuses++;

        if (_currentFuses == _maxFuses)
            GameEvent.Instance.ShieldFixed?.Invoke();
    }
}
