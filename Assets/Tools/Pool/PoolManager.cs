using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour {

	private static PoolManager m_instance;
	public static PoolManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance=GameObject.Instantiate<PoolManager>(Resources.Load<PoolManager>("Pool/PoolManager"));
//				DontDestroyOnLoad(m_instance.gameObject);
//				foreach (Pool pool in m_instance.Pools)
//				{
//					pool.BuildPool();
//				}
			}
			return m_instance;
		}
	}
	
	
	public List<Pool> Pools;

	public void Awake()
	{
		if(m_instance != null)
			return;
		m_instance = this;
		DontDestroyOnLoad(gameObject);
		foreach (Pool pool in Pools)
		{
			pool.BuildPool();
		}
	}
	
	public GameObject GetObject(string name,Transform parent = null)
	{
		foreach (Pool pool in Pools)
		{
			if (pool.PoolName == name)
			{
				PooledObject obj = pool.GetObject();
				if (obj == null)
					return null;
				GameObject retour = obj.gameObject;
				if (parent != null)
				{
					retour.transform.SetParent(parent,false);
					retour.transform.localPosition = Vector3.zero;
					retour.transform.localScale =Vector3.one;
					
					//retour.transform.position = parent.transform.position;
					//retour.transform.localScale = parent.transform.localScale;
				}
				retour.SetActive(true);
				return retour;
			}
		}
		Debug.LogError("PoolNotSet: " + name);
		return null;
	}
	
	public GameObject GetObject(GameObject reference)
	{
		return this.GetObject(reference.name);
	}

	public void Refill(PooledObject pooledObject)
	{
		StartCoroutine(RefillCoroutine(pooledObject));
	}

	private IEnumerator RefillCoroutine(PooledObject pooledObject)
	{
		yield return new WaitForEndOfFrame();
        pooledObject.transform.parent = transform;
	}
}
