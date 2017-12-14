using UnityEngine;

public interface IDamageable
{
    void Hit(int damages, Vector2 pushForce);
}
