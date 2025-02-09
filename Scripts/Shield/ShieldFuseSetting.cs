using Godot;
using Godot.Collections;

public partial class ShieldFuseSetting : Node
{
    [Export] private Array<Node3D> _fuses;

    [ExportGroup("Audio")]
    [Export] private AudioPlayer3D _audioPlayer3D;
    [Export] private AudioStreamMP3 _setFuseAudio;
    [Export] private AudioStreamMP3 _fixShieldAudio;

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
        _audioPlayer3D.PlayAudio3D(_setFuseAudio);
        _currentFuses++;

        if (_currentFuses == _maxFuses)
        {
            GameEvent.Instance.ShieldFixed?.Invoke();
            _audioPlayer3D.PlayAudio3D(_fixShieldAudio);
        }
    }
}
