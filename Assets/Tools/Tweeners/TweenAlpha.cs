using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TweenAlpha : TweenBase {

	public float From;
	public float To;

	public static TweenAlpha AddTween(GameObject target,float from,float to,float duration)
	{
		List<Keyframe> keys = new List<Keyframe>();
		keys.Add(new Keyframe(0,0));
		keys.Add(new Keyframe(1,1));
		return AddTween(target,from,to,duration,keys.ToArray());
	}
	public static TweenAlpha AddTween(GameObject target,float from,float to,float duration, Keyframe[] keys )
	{

		TweenAlpha tweener = target.AddComponent<TweenAlpha>();
		tweener.Target = target;
		tweener.From = from;
		tweener.To = to;
		tweener.Duration = duration;
		tweener.Curve = new AnimationCurve();
		tweener.Curve.keys = keys;

		tweener.Initialise();

		return tweener;
	}

	public override void Initialise()
	{
		m_maskable = Target.GetComponent<MaskableGraphic>();
		m_group = Target.GetComponent<CanvasGroup>();
		m_sprite = Target.GetComponent<SpriteRenderer>();
		base.Initialise();
	}

	
	public override void ApplyModification (float evaluation)
	{
		if(m_initialised == false)
			Initialise();
		//float multipiler = Curve.Evaluate (progress);
		if (m_group != null)
		{
			float currentColor =m_group.alpha;
			currentColor = Mathf.Lerp(From,To,evaluation);
			m_group.alpha = currentColor;
		}
		else
		{
			Color currentColor = m_maskable != null? m_maskable.color:m_sprite.color;
			currentColor.a = Mathf.Lerp(From,To,evaluation);
			if( m_maskable != null)
				m_maskable.color=currentColor;
			else
				m_sprite.color = currentColor;
		}
	}

	private MaskableGraphic m_maskable;
	private CanvasGroup m_group;
	private SpriteRenderer m_sprite;
}
