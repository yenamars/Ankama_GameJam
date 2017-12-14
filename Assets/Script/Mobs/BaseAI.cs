using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : Actor
{
	[Header("BaseAI")] 
	[HideInInspector] public Vector3 Direction;
    public float maxSpeed;

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

		//m_rigidbody.velocity = Direction*Speed;
        Vector3 force = Direction*Speed;
        if (force.magnitude < maxSpeed)
        {
            m_rigidbody.AddForce(force, ForceMode2D.Force);
        }
            
		Vector3 LookTarget = m_target.transform.position;
		LookTarget.z = transform.position.z;
		Vector3 difPosition = LookTarget - transform.position;

		float angle = Mathf.Atan(difPosition.y / difPosition.x);
		Arm.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg*angle -(difPosition.x > 0?90:-90));
	}

    public override void Hit(int damages, Vector2 pushForce)
    {
        m_lifePoint -= damages;
        if (m_lifePoint <= 0)
        {
            OnDeath();
        }

        m_rigidbody.AddForce(pushForce, ForceMode2D.Impulse);
    }

	private GameObject m_target;
}
