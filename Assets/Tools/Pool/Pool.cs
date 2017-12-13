using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Pool 
{

	public string PoolName;
	public int ItemCount;
	public PooledObject PrefabReference;
	[HideInInspector]
	public List<PooledObject> AllObject;

	public void BuildPool()
	{
		m_freeObject = new List<PooledObject>();
		for (int i = 0; i < ItemCount; i++)
		{
			AllObject.Add(GameObject.Instantiate<PooledObject>(PrefabReference));
			AllObject[i].Initialise(this);
		}
	}
	
	public PooledObject GetObject()
	{
		if (m_freeObject.Count > 0)
		{
			PooledObject obj = m_freeObject[0];
			m_freeObject.RemoveAt(0);
			return obj;
		}
		else
		{
			Debug.LogError("Not enought item : " + PoolName);
		}
		return null;
	}
	
	public void RefillObject(PooledObject pooledObject)
	{
		m_freeObject.Add(pooledObject);
	}
	
	private List<PooledObject> m_freeObject;

}
