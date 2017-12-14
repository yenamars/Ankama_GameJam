using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{


	[Header("Actor")] public int LifePoint = 10;
	
	public GameObject Arm;
	
	public void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_mainCamera = Camera.main;
	}
	
	
	protected Camera m_mainCamera;
	protected Rigidbody2D m_rigidbody;
}
