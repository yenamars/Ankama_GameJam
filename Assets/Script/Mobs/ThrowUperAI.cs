using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowUperAI : BaseAI
{

	[Header("ThrowUpper")] public BaseWeapon Weapon;

	public float ShootRange = 5;
	
	public override void Update()
	{
		if (isActive == false || isAlive == false)
			return;

		Vector3 diffToPlayer = m_target.transform.position - transform.position;

		diffToPlayer.z = 0;
		if(diffToPlayer.magnitude < ShootRange)
			Weapon.Shoot();
		else
		{
			Weapon.StopShoot();
		}
		base.Update();
	}
}
