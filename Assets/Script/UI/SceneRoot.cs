
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour
{


	public GameObject MobeRoot;
	public LevelState State;

	[Header("Test")] public bool AutoStart;


	public void AutoStar()
	{
		
		SceneManager.LoadScene("Common", LoadSceneMode.Additive);
		AutoStart = false;
	}
	
	public void StartScene()
	{
		MobeRoot.SetActive(true);
		State = LevelState.Play;
	}

	public void Update()
	{
		if (AutoStart)
		{
			AutoStar();
		}
		if (MobeRoot.GetComponentsInChildren<BaseAI>().Length == 0 && State == LevelState.Play) 
		{
			State = LevelState.Outro;
			OnSceneClear();
		}
	}

	public void OnSceneClear()
	{
		SceneControler.Instance.LoadNextScene();
	}

}

public enum LevelState
{
	Intro,
	Play,
	Outro
}