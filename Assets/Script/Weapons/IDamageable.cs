public enum HitType
{
    Shot = 0,
    Collision = 1,
    InstaKill = 2
}

public interface IDamageable
{
    void Hit(int damages, HitType hitType);
}
