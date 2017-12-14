using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleProjectile : MonoBehaviour 
{   
    [SerializeField] private int damages;
    [SerializeField] private LayerMask damagesMask;
    [SerializeField] private ParticleSystem pSystem;
    [SerializeField] private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private ParticleSystem.Particle[] particles;
    private Transform trsf;

    void Awake()
    {
        particles = new ParticleSystem.Particle[pSystem.main.maxParticles];
        trsf = transform;
    }

    public void Shoot(int count, Transform cannon)
    {
        trsf.localPosition = cannon.localPosition;
        trsf.localRotation = cannon.localRotation;
        pSystem.Emit(count);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.layer == 12)
            return;
        
        int collisionsCount = pSystem.GetCollisionEvents(other, collisionEvents);
        
        if(damagesMask == (damagesMask | (1 << other.layer)))
        {
            IDamageable d = other.GetComponent<IDamageable>();

            if (d != null)
            {
                d.Hit (damages, HitType.Shot);
            }
        }
    }

    public void OnPlayerDeath()
    {
        pSystem.Clear();
    }
}
