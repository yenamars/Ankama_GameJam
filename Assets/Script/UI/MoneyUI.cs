using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{

	public Text MoneyCount;
	
	// Update is called once per frame
	void Update ()
	{
		MoneyCount.text = MoneyManager.instance.currentScore.ToString()+"$";
	}
}
