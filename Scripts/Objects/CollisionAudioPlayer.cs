using Godot;

public partial class CollisionAudioPlayer : RigidBody3D
{
    [Export] private AudioStreamMP3 HitSound;
    [Export] private AudioStreamMP3 SlideSound;
    [Export] private float MinImpactSpeed = 5f;
    [Export] private float MinSlideSpeed = 1f;
    [Export] private float SlideNormalThreshold = 0.8f;

    [Export]  private AudioStreamPlayer3D audioPlayer;

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        for (int i = 0; i < state.GetContactCount(); i++)
        {
            Vector3 contactPoint = state.GetContactLocalPosition(i);
            Vector3 normal = state.GetContactLocalNormal(i);
            float impulse = state.GetContactImpulse(i).Length();
            Vector3 relativeVelocity = state.GetContactLocalVelocityAtPosition(i);

            if (impulse > MinImpactSpeed)
            {
                PlaySound(HitSound, volume: impulse / 10f);
                return;
            }

            if (relativeVelocity.Length() > MinSlideSpeed)
            {
                float angle = normal.Dot(Vector3.Up);
                if (angle < SlideNormalThreshold)
                {
                    PlaySound(SlideSound, volume: relativeVelocity.Length() / 10f);
                }
            }
        }
    }

    private void PlaySound(AudioStreamMP3 sound, float volume = 1f)
    {
        if (sound == null) return;

        audioPlayer.Stream = sound;
        audioPlayer.VolumeDb = Mathf.LinearToDb(volume);
        audioPlayer.Play();
    }
}
