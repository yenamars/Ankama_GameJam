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
    [SerializeField] private Transform bloodFX;
    [SerializeField] protected AudioSource shootSound;
    [SerializeField] protected Vector2 shootSoundRandomPitch;
    [SerializeField] protected AudioSource viandeSound;
    [SerializeField] protected Vector2 viandeSoundRandomPitch;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private Transform trsf;
    private ParticleSystem[] flameFXs;
    private ParticleSystem[] impactFXs;
    private ParticleSystem[] bloodFXs;

    void Awake()
    {
        trsf = transform;

        if(flameFX != null)
            flameFXs = flameFX.gameObject.GetComponentsInChildren<ParticleSystem>();

        if(impactFX != null)
            impactFXs = impactFX.gameObject.GetComponentsInChildren<ParticleSystem>();

        if(bloodFX != null)
            bloodFXs = bloodFX.gameObject.GetComponentsInChildren<ParticleSystem>();
    }

    public void Shoot(int count, Transform cannon)
    {
        trsf.position = cannon.position;
        trsf.rotation = cannon.rotation;
        pSystem.Emit(count);

        if (flameFX != null)
        {
            for (int i = 0; i < flameFXs.Length; i++)
            {
                flameFXs[i].Play(false);
            }
        }

        if (shootSound != null)
        {
            shootSound.pitch = Random.Range(shootSoundRandomPitch.x, shootSoundRandomPitch.y);
            shootSound.Play();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int collisionsCount = pSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < collisionsCount; i++)
        {
            if (impactFX == null)
                break;
            
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

                    if (other.layer == 9)
                    {
                        if (bloodFX != null)
                        {
                            bloodFX.position = collisionEvents[i].intersection;
                            for (int j = 0; j < bloodFXs.Length; j++)
                            {
                                bloodFXs[j].Play(false);
                            }
                        }

                        if (viandeSound != null)
                        {
                            viandeSound.pitch = Random.Range(viandeSoundRandomPitch.x, viandeSoundRandomPitch.y);
                            viandeSound.Play();
                        }
                    }
                }
            }
        }
    }
}
