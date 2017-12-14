using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Actor : MonoBehaviour,IDamageable
{


	[Header("Actor")] public int LifePoint = 10;
	
	[HideInInspector] public Vector3 Direction;
	public float Speed;
	public GameObject Arm;
	
	public void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_mainCamera = Camera.main;
		m_lifePoint = LifePoint;
	}

	public virtual void Update()
	{
		m_stoppedTimer -= Time.deltaTime;
	}
	
	protected void SetVelocity()
	{
		if (m_stoppedTimer > 0)
		{
			m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
			m_rigidbody.velocity = Vector2.zero;
			return;
		}

		m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
		if (Direction.magnitude > 1)
		{
			float magnitude = Direction.magnitude;
			Direction.x /= magnitude;
			Direction.y /= magnitude;
		}
		m_rigidbody.velocity = Direction * Speed;
	}
	public void Hit(int damages, HitType hitType)
	{
		m_lifePoint -= damages;
		if (m_lifePoint <= 0)
		{
			OnDeath();
		}
	}
	public void StopFor(float i)
	{
		m_stoppedTimer = i;
	}
	public void OnDeath()
	{
		GameObject.Destroy(gameObject);
	}

	protected float m_stoppedTimer;
	
	protected Camera m_mainCamera;
	protected Rigidbody2D m_rigidbody;
	protected int m_lifePoint;

	
}
