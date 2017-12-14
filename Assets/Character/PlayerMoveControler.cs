using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControler : MonoBehaviour
{

	public float Speed;
	public GameObject Arm;
	
	[HideInInspector] public Vector3 Direction;
	[HideInInspector] public Vector3 Orientation;

	public void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_mainCamera = Camera.main;
	}

	public void Update()
	{
		Direction = new Vector3( Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
		if (Direction.magnitude > 1)
		{
			float magnitude = Direction.magnitude;
			Direction.x /= magnitude;
			Direction.y /= magnitude;
		}
		m_rigidbody.velocity = Direction*Speed;

		Vector3 LookTarget = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
		LookTarget.z = transform.position.z;
		Vector3 difPosition = LookTarget - transform.position;

		float angle = Mathf.Atan(difPosition.y / difPosition.x);
		Arm.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg*angle -(difPosition.x > 0?90:-90));
	}

	private Camera m_mainCamera;
	private Rigidbody2D m_rigidbody;
}
