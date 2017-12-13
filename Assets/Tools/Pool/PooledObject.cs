using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject:MonoBehaviour
{
	[HideInInspector]
	public Pool m_parentPool;

	public void Initialise(Pool parentPool)
	{
		
		transform.SetParent(PoolManager.instance.transform);
		m_parentPool = parentPool;
		gameObject.SetActive(false);
		//DeletePoolobject();
	}
	
	public void DeletePoolobject()
	{
		PoolManager.instance.Refill(this);
		//transform.SetParent(PoolManager.instance.transform);
		m_parentPool.RefillObject(this);
	}

	public void OnDisable()
	{
		DeletePoolobject();
	}
	
}
