using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour,IDamageable
{


	[Header("Actor")] public int LifePoint = 10;
	
	public float Speed;
	public GameObject Arm;
	
	public void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_mainCamera = Camera.main;
		m_lifePoint = LifePoint;
	}
	
	public void Hit(int damages, HitType hitType)
	{
		m_lifePoint -= damages;
		if (m_lifePoint <= 0)
		{
			OnDeath();
		}
	}

	public void OnDeath()
	{
		GameObject.Destroy(gameObject);
	}
	
	protected Camera m_mainCamera;
	protected Rigidbody2D m_rigidbody;
	protected int m_lifePoint;

}
