using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DepthSorter : MonoBehaviour {

	public bool ApplyOnUpdate = false;
	public bool ForceApply; 
	public bool CompressOrders = false;
	public int Offset;

	// Use this for initialization
	void Start () 
    {
		SetDepth (CompressOrders);
	
   	}


	void Update () 
	{
		if(ApplyOnUpdate || ForceApply)
		{
			SetDepth(CompressOrders);
			ForceApply = false;
		}
	}

	void SetDepth (bool compressOrder = false)
	{
		//SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer> ();
		List<SpriteRenderer> sprites = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer> ());
		foreach (SpriteRenderer sprite in sprites) 
		{
//			if(sprite.name == "Tower_Locked_10")
////				Debug.Log(sprite.sortingOrder);
			sprite.sortingOrder = (int)(-sprite.transform.position.y * 10 + Offset);

//			if(sprite.name == "Tower_Locked_10")
//				Debug.Log(sprite.sortingOrder);
		}
		foreach (SpriteDepthModifier modifier in GetComponentsInChildren<SpriteDepthModifier>()) 
		{
			modifier.Apply();
		}
		if(compressOrder)
		{
			if(m_comparer ==null)
				m_comparer = new SpriteRendererCompararer();
			sprites.Sort(m_comparer);

			for(int i=0;i<sprites.Count;i++)
			{
				sprites[i].sortingOrder = i;

//				if(sprites[i].name == "Tower_Locked_10")
//					Debug.Log(sprites[i].sortingOrder);
			}
		}

	}
		
	private SpriteRendererCompararer m_comparer;
}

public class SpriteRendererCompararer: IComparer<SpriteRenderer>
{
	#region IComparer implementation

	public int Compare (SpriteRenderer x, SpriteRenderer y)
	{
		return x.sortingOrder - y.sortingOrder;
	}

	#endregion
}
