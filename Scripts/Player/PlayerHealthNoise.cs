using Godot;

public partial class PlayerHealthNoise : Node
{
    [Export] private PlayerHealth _playerHealth;
    [Export] private AudioPlayer _audioPlayer;
    [Export] private AudioStreamMP3 _healthNoise;

    [Export] private int _healthThreshold = 50;
    [Export] private float _minVolumeDb = 0f;
    [Export] private float _maxVolumeDb = 5f;

    private bool _isPlaying = false;

    public override void _Ready()
    {
        _playerHealth.PlayerHealthChanged += OnHealthChanged;
    }

    public override void _ExitTree()
    {
        _playerHealth.PlayerHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        int currentHealth = _playerHealth.GetCurrentHealth();

        if (currentHealth <= _healthThreshold)
            PlayNoise(currentHealth);
        else if (_isPlaying)
            StopNoise();
    }

    private void PlayNoise(int currentHealth)
    {
        if (!_isPlaying)
        {
            _audioPlayer.PlayAudio(_healthNoise);
            _isPlaying = true;
        }

        float healthFactor = Mathf.Clamp((float)currentHealth / _healthThreshold, 0f, 1f);
        float volumeDb = Mathf.Lerp(_maxVolumeDb, _minVolumeDb, healthFactor);
        _audioPlayer.VolumeDb = volumeDb;
    }

    private void StopNoise()
    {
        _audioPlayer.Stop();
        _isPlaying = false;
    }
}
