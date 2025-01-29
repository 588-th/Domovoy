using Godot;

public partial class PlayerMeleeAttackParameters : Resource
{
    [Export] public float AttackCooldownTime;
    [Export] public int LightAttackDamage;

    [Export] public AudioStreamMP3 MissAttackAudio;
    [Export] public AudioStreamMP3 HitAttackAudio;
}
