using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleProjectile : MonoBehaviour 
{   
    [SerializeField] private int damages;
    [SerializeField] private LayerMask damagesMask;
    [SerializeField] private float pushForce;
    [SerializeField] private ParticleSystem pSystem;
    [SerializeField] private ParticleSystem flameFX;
    [SerializeField] private Transform impactFX;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private Transform trsf;
    private ParticleSystem[] flameFXs;
    private ParticleSystem[] impactFXs;

    void Awake()
    {
        trsf = transform;
        flameFXs = flameFX.gameObject.GetComponentsInChildren<ParticleSystem>();
        impactFXs = impactFX.gameObject.GetComponentsInChildren<ParticleSystem>();
    }

    public void Shoot(int count, Transform cannon)
    {
        trsf.position = cannon.position;
        trsf.rotation = cannon.rotation;
        pSystem.Emit(count);

        for (int i = 0; i < flameFXs.Length; i++)
        {
            flameFXs[i].Play(false);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int collisionsCount = pSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < collisionsCount; i++)
        {
            impactFX.position = collisionEvents[i].intersection;
            for (int j = 0; j < impactFXs.Length; j++)
            {
                impactFXs[j].Play(false);
            }
        }

        if (damagesMask == (damagesMask | (1 << other.layer)))
        {
            IDamageable d = other.GetComponent<IDamageable>();
            if (d != null)
            {
                for (int i = 0; i < collisionsCount; i++)
                {
                    d.Hit(damages, collisionEvents[i].velocity.normalized * pushForce);
                }
            }
        }
    }
}
