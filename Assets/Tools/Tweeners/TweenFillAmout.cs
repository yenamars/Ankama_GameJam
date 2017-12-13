using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenFillAmout : TweenBase {

	public float From;
	public float To;
	public static TweenFillAmout AddTween(GameObject target,float from,float to,float duration)
	{
		List<Keyframe> keys = new List<Keyframe>();
		keys.Add(new Keyframe(0,0));
		keys.Add(new Keyframe(1,1));
		return AddTween(target,from,to,duration,keys.ToArray());
	}
	public static TweenFillAmout AddTween(GameObject target,float from,float to,float duration, Keyframe[] keys )
	{

		TweenFillAmout tweener = target.AddComponent<TweenFillAmout>();
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
		m_imageTarget = Target.GetComponent<Image>();
		base.Initialise();
	}
	public override void ApplyModification (float evaluation)
	{
		//float interpolation = Curve.Evaluate (progress);

		float fromWeight = Mathf.Max (1-evaluation,-10);
		float toWeight = Mathf.Max (evaluation,-10);
		
//		float x = (To.x - From.x)*evaluation;
//		float y = (To.y - From.y)*evaluation;
//		float z = (To.z - From.z)*evaluation;

		float median = To*toWeight+From*fromWeight;
		m_imageTarget.fillAmount = median;//new Vector3 (x, y, z);
	}

	private Image m_imageTarget;
}
