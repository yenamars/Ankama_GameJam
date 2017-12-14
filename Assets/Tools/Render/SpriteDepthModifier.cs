using UnityEngine;
using System.Collections;

public class SpriteDepthModifier : MonoBehaviour {

	public SpriteRenderer ParentSprite;
	public int Offset = 1;
	public SpriteRenderer Target;
	
	public void Awake()
	{
		if(Target == null)
			m_sprite = GetComponent<SpriteRenderer>();
		else
		{
			m_sprite = Target;
		}
	}

	public void Apply()
	{
		if(ParentSprite == null)
			return;
		if(m_sprite == null)
			m_sprite = GetComponent<SpriteRenderer>();
		m_sprite.sortingOrder = ParentSprite.sortingOrder + Offset;
	}

	public void SetTarget(SpriteRenderer visualRenderer)
	{
		Target = visualRenderer;
		m_sprite = visualRenderer;
	}
	private SpriteRenderer m_sprite;

}
