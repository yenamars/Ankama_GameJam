using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTrap : BaseTrap
{

	[Header("AOE")] public float range;

	protected override void ApplyEffect(TrapTarget trapped)
	{
        Instantiate(trapFX, transform);
        StackableShake.instance.Shake(trapShake);

		foreach (Actor actor in m_mobRoot.GetComponentsInChildren<Actor>())
		{
			Vector3 distanceToTrap = actor.transform.position - transform.position;
			distanceToTrap.z = 0;
            if(distanceToTrap.sqrMagnitude < range*range)
				actor.Hit(power,Vector2.zero);
		}
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
