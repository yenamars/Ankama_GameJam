using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	
	private bool click;
	private bool loading;
	void Awake ()
	{
		SceneManager.LoadScene("Common", LoadSceneMode.Additive);
		
		MoneyManager.instance.currentScore = 0;
		if (SceneControler.Instance == null)
		{
			SceneManager.LoadScene("Splash",LoadSceneMode.Additive);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			click = true;
		}
		else
		{
			click = false;
		}

		if (click == true && loading == false)
		{
			loading = true;
			StartGame();
		}
	}
	public void StartGame()
	{
		SceneControler.Instance.StartGame();
	}

}
