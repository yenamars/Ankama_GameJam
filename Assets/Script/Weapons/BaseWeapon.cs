using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour 
{
    [SerializeField] protected ParticleProjectile particleProjectile;
    [SerializeField] protected Transform[] cannons;
    [SerializeField] protected int bulletsPerShot;
    [SerializeField] protected float shootRateBase;
    [SerializeField] protected float delayBetweenCannons;
    public int id;

    [HideInInspector] public bool shooting;
    protected float timer;
    protected float secondaryTimer;
    protected int cannonIndex;
    protected int cannonsCount;
    protected float shootRate;
    protected float cannonsLength;
    protected WaitForSeconds waitShootRate;
    protected WaitForSeconds waitDelayBetweenCannons;
    protected Coroutine shootCoroutine;

    protected virtual void Awake()
	{
        shootRate = shootRateBase;
        waitShootRate = new WaitForSeconds(shootRate);
        waitDelayBetweenCannons = new WaitForSeconds(delayBetweenCannons);
        cannonsLength = cannons.Length;
	}

    protected virtual void OnEnable()
	{
        timer = 0.0f;
        cannonsCount = cannons.Length;
        secondaryTimer = delayBetweenCannons;
        cannonIndex = 1;
        shooting = false;
        shootCoroutine = null;
	}

    public virtual void Shoot()
    {
        if (shooting == false)
        {
            shooting = true;

            if(shootCoroutine == null)
                shootCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        while (shooting == true)
        {
            for (int i = 0; i < cannonsLength; i++)
            {
                particleProjectile.Shoot(bulletsPerShot, cannons[i]);

                if (delayBetweenCannons > 0.0f)
                {
                    yield return waitDelayBetweenCannons;
                }
            }

            yield return waitShootRate;
        }

        shootCoroutine = null;
    }

    public virtual void StopShoot()
    {
        shooting = false;

//        if (shootCoroutine != null)
//        {
//            StopCoroutine(shootCoroutine);
//            shootCoroutine = null;
//        }

    }
}
