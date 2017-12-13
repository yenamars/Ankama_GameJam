using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestrucTimer : MonoBehaviour
{
	
	public float TimeDelay;
	void Start () {
		
	}

	public void OnEnable()
	{
		m_curentTimer = TimeDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_curentTimer -= Time.deltaTime;

		if (m_curentTimer < 0)
		{
			PooledObject pooled = GetComponent<PooledObject>();
			if (pooled != null)
			{
				pooled.gameObject.SetActive(false);
				//pooled.DeletePoolobject();
				
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

	private float m_curentTimer;
}
