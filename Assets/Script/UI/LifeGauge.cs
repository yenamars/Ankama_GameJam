using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGauge : MonoBehaviour
{

	public Image Gauge;
	public void Awake()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveControler>().LifeGauge = Gauge;
	}
}
