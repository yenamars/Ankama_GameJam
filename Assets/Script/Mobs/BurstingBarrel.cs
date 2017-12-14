using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstingBarrel : Actor
{

	[Header("Barrel")] public int Strength = 200;
	public float range = 1.5f;
	
	public void Awake()
	{
		base.Awake();
		m_mobRoot = GameObject.FindGameObjectWithTag("MobRoot");
	}
	public override void OnDeath()
	{
		base.OnDeath();
		foreach (Actor actor in m_mobRoot.GetComponentsInChildren<Actor>())
		{
			Vector3 distanceToTrap = actor.transform.position - transform.position;
			distanceToTrap.z = 0;
			if(distanceToTrap.magnitude < range)
				actor.Hit(Strength,Vector2.zero);
		}
	}
	
	
	protected GameObject m_mobRoot;
}
