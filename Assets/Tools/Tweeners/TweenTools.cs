using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenTools : MonoBehaviour {

	public static Keyframe[] GetGrowthBounceFrames()
	{
		List<Keyframe> keys = new List<Keyframe>();
		keys.Add(new Keyframe(0,1));
		keys.Add(new Keyframe(0.33f,0.8f));
		keys.Add(new Keyframe(0.66f,1.2f));
		keys.Add(new Keyframe(1,1));

		return keys.ToArray();
	}

	public static Keyframe[] GetStampKeys ()
	{
		List<Keyframe> keys = new List<Keyframe>();
		keys.Add(new Keyframe(0,0));
		keys.Add(new Keyframe(0.75f,1.2f));
		keys.Add(new Keyframe(1,1));
		
		return keys.ToArray();
	}
}
