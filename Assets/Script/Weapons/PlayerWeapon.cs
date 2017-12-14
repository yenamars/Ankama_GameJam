using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : BaseWeapon 
{
    [SerializeField] protected StackableShakeData shakeData;
    protected StackableShake stackableShake;

    protected override void Awake()
    {
        base.Awake();

        if (shakeData != null)
        {
            stackableShake = StackableShake.instance;
        }
    }

    public override void Shoot()
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

            if (shakeData != null)
            {
                stackableShake.Shake(shakeData);
            }

            yield return waitShootRate;
        }

        shootCoroutine = null;
    }
}
