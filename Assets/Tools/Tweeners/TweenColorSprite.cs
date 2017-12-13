using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TweenColorSprite : TweenBase {
	
	public Color From;
	public Color To;
	
	public static TweenColorSprite AddTween(GameObject target,Color from,Color to,float duration)
	{
		List<Keyframe> keys = new List<Keyframe>();
		keys.Add(new Keyframe(0,0));
		keys.Add(new Keyframe(1,1));
		return AddTween(target,from,to,duration,keys.ToArray());
	}
	public static TweenColorSprite AddTween(GameObject target,Color from,Color to,float duration, Keyframe[] keys )
	{
		
		TweenColorSprite tweener = target.AddComponent<TweenColorSprite>();
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
		m_maskable = Target.GetComponentInChildren<SpriteRenderer>();
		base.Initialise();
	}
	
	
	public override void ApplyModification (float evaluation)
	{
		//float multipiler = Curve.Evaluate (progress);
		base.ApplyModification(evaluation);
		m_maskable.color = Color.Lerp(From,To,evaluation);
	}
	
	private SpriteRenderer m_maskable;
}

