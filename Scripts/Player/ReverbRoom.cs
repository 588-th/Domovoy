using Godot;
using Godot.Collections;

public partial class ReverbRoom : Node
{
    [Export] private CharacterBody3D _playerBody;
    [Export] private Array<RayCast3D> _raycasts = new();

    private float roomSize = 10.0f;

    public override void _PhysicsProcess(double delta)
    {
        //if (_playerBody.Velocity.LengthSquared() == 0)
        //    return;

        //CalculateRoomSize();
        //ApplyReverbEffect();
    }

    private void CalculateRoomSize()
    {
        float totalSize = 0;
        int validRayCount = 0;

        foreach (RayCast3D ray in _raycasts)
        {
            float distance = GetRayDistance(ray);
            if (distance < 50f)
            {
                totalSize += distance;
                validRayCount++;
            }
        }

        if (validRayCount > 0)
        {
            roomSize = totalSize / validRayCount;

            roomSize = Mathf.Pow(roomSize, 2f);
        }
    }

    private float GetRayDistance(RayCast3D ray)
    {
        return ray.IsColliding() ? (ray.GetCollisionPoint() - ray.GlobalTransform.Origin).Length() : 50f;
    }

    private void ApplyReverbEffect()
    {
        int busIndex = AudioServer.GetBusIndex("SFX");

        for (int i = 0; i < AudioServer.GetBusEffectCount(busIndex); i++)
        {
            if (AudioServer.GetBusEffect(busIndex, i) is AudioEffectReverb existingReverb)
            {
                float clampedSize = Mathf.Clamp(roomSize / 100.0f, 0.01f, 1f);
                clampedSize = Mathf.Round(clampedSize * 100f) / 100f;

                float wetAmount = Mathf.Clamp(clampedSize, 0.05f, 1.0f);

                existingReverb.RoomSize = clampedSize;
                existingReverb.Damping = 0.5f;
                existingReverb.Spread = 1f;
                existingReverb.Hipass = 0f;
                existingReverb.Dry = 1f;
                existingReverb.Wet = wetAmount;

                if (Multiplayer.IsServer())
                    GD.Print($"Room Size Detected: {roomSize} | Clamped: {existingReverb.RoomSize} | Wet: {wetAmount}");

                return;
            }
        }
    }
}
