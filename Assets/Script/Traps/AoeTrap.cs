using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTrap : BaseTrap
{

	[Header("AOE")] public float range;

	protected override void ApplyEffect(TrapTarget trapped)
	{
		foreach (Actor actor in m_mobRoot.GetComponentsInChildren<Actor>())
		{
			Vector3 distanceToTrap = actor.transform.position - transform.position;
			distanceToTrap.z = 0;
			if(distanceToTrap.magnitude < range)
				actor.Hit(power,HitType.Shot);
		}
	}
}
