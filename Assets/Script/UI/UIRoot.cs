using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour {

	public Image Gauge;
	public CanvasGroup group;
	
	public void Awake()
	{
		@group.alpha = 0;
		//GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveControler>().LifeGauge = Gauge;
	}
	public void InitialiseUI(PlayerMoveControler player)
	{
		player.LifeGauge = Gauge;
	}
}
