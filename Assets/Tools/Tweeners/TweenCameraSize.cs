using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenCameraSize : TweenBase {

	public float From;
	public float To;

	public static TweenCameraSize AddTween(GameObject target,float from,float to,float duration)
	{
		List<Keyframe> keys = new List<Keyframe>();
		keys.Add(new Keyframe(0,0));
		keys.Add(new Keyframe(1,1));
		return AddTween(target,from,to,duration,keys.ToArray());
	}
	public static TweenCameraSize AddTween(GameObject target,float from,float to,float duration, Keyframe[] keys )
	{

		TweenCameraSize tweener = target.AddComponent<TweenCameraSize>();
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
		m_camera = Target.GetComponent<Camera>();
		base.Initialise();
	}

	
	public override void ApplyModification (float evaluation)
	{
		if(m_initialised == false)
			Initialise();
		//float multipiler = Curve.Evaluate (progress);
		float currentSize =Mathf.Lerp(From,To,evaluation);
		m_camera.orthographicSize = currentSize;

	}

	private Camera m_camera;
}
