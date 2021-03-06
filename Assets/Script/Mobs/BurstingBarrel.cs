﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstingBarrel : Actor
{
	[Header("Barrel")] 
    public int Strength = 200;
    public float push = 1.0f;
    public LayerMask damagesLayer;
    public ParticleSystem flamesFX;

    [Header("Shake")] public StackableShakeData destroyShake;
	public float range = 1.5f;
    private ParticleSystem.EmissionModule emissionModule;

	public void Awake()
	{
		base.Awake();
		m_mobRoot = GameObject.FindGameObjectWithTag("MobRoot");
        emissionModule = flamesFX.emission;
        emissionModule.enabled = false;
        emissionModule.rateOverTime = 0.0f;
	}

    public override void Hit(int damages, Vector2 pushForce)
    {
        base.Hit(damages, pushForce);
        m_rigidbody.AddForce(pushForce, ForceMode2D.Impulse);
        emissionModule.enabled = true;
        emissionModule.rateOverTime = (LifePoint - m_lifePoint) * 3.0f;
    }

	public override void OnDeath()
	{
		StartCoroutine(Burst());
	}

	private IEnumerator Burst()
	{
        yield return new WaitForSeconds(0.2f);
		StackableShake.instance.Shake(destroyShake);

		Collider2D[] colls = Physics2D.OverlapCircleAll(m_rigidbody.position, range, damagesLayer, -1.0f, 1.0f);
            
		for (int i = 0; i < colls.Length; i++)
		{
			IDamageable d = colls[i].GetComponent<IDamageable>();

			if (d != null)
			{
				Vector3 p = colls[i].transform.position;
				d.Hit(Strength, -(m_rigidbody.position - new Vector2(p.x, p.y)).normalized * push);
			}
		}
		base.OnDeath();
	}

	void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

	protected GameObject m_mobRoot;
}
