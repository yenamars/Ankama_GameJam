using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TweenRotation : TweenBase {

	public Vector3 From;
	public Vector3 To;

	public static TweenRotation AddTween(GameObject target,Vector3 from,Vector3 to,float duration)
	{
		List<Keyframe> keys = new List<Keyframe>();
		keys.Add(new Keyframe(0,0));
		keys.Add(new Keyframe(1,1));
		return AddTween(target,from,to,duration,keys.ToArray());
	}
	public static TweenRotation AddTween(GameObject target,Vector3 from,Vector3 to,float duration, Keyframe[] keys )
	{

		TweenRotation tweener = target.AddComponent<TweenRotation>();
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
		m_maskable = GetComponent<MaskableGraphic>();
	}

	
	public override void ApplyModification (float evaluation)
	{
		//float multipiler = Curve.Evaluate (progress);
		float fromWeight = Mathf.Max (1-evaluation,0);
		float toWeight = Mathf.Max (evaluation,0);
		
		float x = (To.x - From.x)*evaluation;
		float y = (To.y - From.y)*evaluation;
		float z = (To.z - From.z)*evaluation;
		
		Vector3 median = To*toWeight+From*fromWeight;
		Target.transform.localRotation = Quaternion.Euler( median);//new Vector3 (x, y, z);
	}

	private MaskableGraphic m_maskable;
	//private float m_currentTime;
}
