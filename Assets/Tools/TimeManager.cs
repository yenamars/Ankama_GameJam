using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	
	private static TimeManager m_instance;
	public static TimeManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance=GameObject.Instantiate<TimeManager>(Resources.Load<TimeManager>("Time/TimeManager"));
				DontDestroyOnLoad(m_instance.gameObject);
			}
			return m_instance;
		}
	}

	public void Update()
	{
		m_timeScaleDuration -= Time.unscaledDeltaTime;
		if (m_timeScaleDuration < 0)
			Time.timeScale = m_defaultTimeScale;
	}

	public void SetTimeScale(float scale, float duration = float.MaxValue)
	{
		Time.timeScale = scale;
		m_timeScaleDuration = duration;
	}
	
	public void SetDefaultTimeScale(float scale)
	{
		m_defaultTimeScale = scale;
	}

	private float m_defaultTimeScale=1;
	private float m_timeScaleDuration;

}
