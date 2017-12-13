using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TweenBase : MonoBehaviour {
	
	public GameObject Target;
	public bool DestroyOnFinish = true;
	public bool Loop = false;
	public float Duration;
	public AnimationCurve Curve;

	public Action OnEndAction;

	public bool AutoAdvance = true;
	public bool UnscaledTime = false;
	
	public virtual void Initialise()
	{
		m_initialised = true;
	}
	
	public void SetCurveKeys(Keyframe[] Keys )
	{
		Curve.keys = Keys;
	}
	
	public void Update()
	{
		if (AutoAdvance)
		{
			if (UnscaledTime)
			{
				Advance(Time.unscaledDeltaTime);
			}
			else
			{
				Advance(Time.deltaTime);
			}
		}
	}

	public void Advance(float dt)
	{
		float evaluation = Curve.Evaluate (m_currentTime);
		ApplyModification (evaluation);
		
		m_currentTime += dt/Duration;
		if(m_currentTime > 1)
		{
			if(Loop)
			{ 
				m_currentTime -=1;
				ApplyModification( Curve.Evaluate (m_currentTime));

			}else
			{
				ApplyModification( Curve.Evaluate (1));
				if(OnEndAction != null)
					OnEndAction.Invoke();
				if(DestroyOnFinish)
					Destroy(this);
				else
				{
					m_currentTime =0;
					enabled = false;
				}
			}
		}
	}
	
	public virtual void ApplyModification (float evaluation)
	{
		if(m_initialised == false)
			Initialise();
	}
	
	protected bool m_initialised;
	private float m_currentTime;
}
