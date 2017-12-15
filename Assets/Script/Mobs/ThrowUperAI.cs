using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowUperAI : BaseAI
{

	[Header("ThrowUpper")] public BaseWeapon Weapon;

	public float ShootRange = 5;
    private bool endShoot;
    private bool shooting;

    public override void Awake()
    {
        base.Awake();
        shooting = false;
        endShoot = false;
    }

	public override void Update()
	{
        base.Update();

		if (isActive == false || isAlive == false)
			return;

		Vector3 diffToPlayer = m_target.transform.position - transform.position;

		diffToPlayer.z = 0;
        if (diffToPlayer.magnitude < ShootRange && shooting == false)
            animator.SetTrigger("Shoot");
	}

    public void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }

    public void EndShoot()
    {
        endShoot = true;
    }

    IEnumerator ShootCoroutine()
    {
        endShoot = false;
        isActive = false;
        shooting = true;
        m_rigidbody.velocity = new Vector2(0.0f, 0.0f);

        Weapon.Shoot();

        while (endShoot == false)
        {
            yield return null;
        }

        Weapon.StopShoot();
        isActive = true;
        shooting = false;
    }

    public override void OnDeath()
    {
        Weapon.StopShoot();
        base.OnDeath();
    }
}
