using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Openning : MonoBehaviour
{

	public GameObject Logo;
	public GameObject Naration;

	public void Awake()
	{
		Naration.GetComponent<Image>().color = new Color(1,1,1,0);
		Logo.GetComponent<Image>().color = new Color(1,1,1,1);
		//TweenAlpha.AddTween(Naration, 1, 0, 0.3f);
		FreezeTimer = 0.5f;
		Swap = 2;
	}
	
	public void Update()
	{
		FreezeTimer -= Time.deltaTime;
		Swap -= Time.deltaTime;
		if (Swap < 0)
		{
			if (m_onNaration == false)
			{
				
				FreezeTimer = 1;
				Swap = 10;
				m_onNaration = true;
				StartCoroutine(ToNaration());
			}
		}
	}


	public void OnClic()
	{
		if(FreezeTimer > 0)
			return;

		if (m_onNaration)
		{
			FreezeTimer = int.MaxValue;
			TweenAlpha.AddTween(Naration, 1, 0, 0.3f);
			StartCoroutine(LoadScene());
		}
		else
		{
			FreezeTimer = 1;
			m_onNaration = true;
			StartCoroutine(ToNaration());
		}
	}

	private IEnumerator ToNaration()
	{
		TweenAlpha.AddTween(Logo, 1, 0, 0.3f);
		yield return new WaitForSeconds(0.3f);
		TweenAlpha.AddTween(Naration, 0, 1, 0.3f);
	}

	private IEnumerator LoadScene()
	{
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("Title");
	}

	public float FreezeTimer;
	public float Swap;

	private bool m_onNaration;
}

