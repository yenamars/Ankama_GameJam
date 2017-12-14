using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : Actor
{


	[Header("BaseAI")] public float Speed = 7;

	[HideInInspector] public Vector3 Direction;

	public void Awake()
	{
		m_target =GameObject.FindGameObjectWithTag("Player");
		base.Awake();
	}
	
	public void Update()
	{
		Direction = m_target.transform.position - transform.position;
		if (Direction.magnitude > 1)
		{
			float magnitude = Direction.magnitude;
			Direction.x /= magnitude;
			Direction.y /= magnitude;
		}
		m_rigidbody.velocity = Direction*Speed;
		
		Vector3 LookTarget = m_target.transform.position;
		LookTarget.z = transform.position.z;
		Vector3 difPosition = LookTarget - transform.position;

		float angle = Mathf.Atan(difPosition.y / difPosition.x);
		Arm.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg*angle -(difPosition.x > 0?90:-90));
	}

	private GameObject m_target;
}
