using Godot;

public partial class FirearmToggleLaser : Node
{
    [Export] private FirearmParameters _firearmParameters;
    [Export] private AudioPlayer _audioPlayer;

    [Export] private SpotLight3D _laserLight;

    private bool _isLaserEnable;

    public void ToggleLaser()
    {
        if (_isLaserEnable)
            ToggleOffLaser();
        else
            ToggleOnLaser();
    }

    private void ToggleOnLaser()
    {
        _laserLight.LightEnergy = _firearmParameters.LaserEnergy;
        _audioPlayer.PlayAudio3D(_firearmParameters.LaserOnAudio);
        _isLaserEnable = true;
    }

    private void ToggleOffLaser()
    {
        _laserLight.LightEnergy = 0;
        _audioPlayer.PlayAudio3D(_firearmParameters.LaserOffAudio);
        _isLaserEnable = false;
    }
}
