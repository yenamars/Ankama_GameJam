
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour
{


	public GameObject MobeRoot;
	public LevelState State;


	public void Start()
	{
		if (SceneControler.Instance == null)
			AutoStar();
	}
	
	public void AutoStar()
	{
		
		SceneManager.LoadScene("Common", LoadSceneMode.Additive);
		GameObject.Instantiate(Resources.Load<PlayerMoveControler>("Player")).SetAlive(true);
		StartScene();
	}
	
	public void StartScene()
	{
		MobeRoot.SetActive(true);
		State = LevelState.Play;
	}

	public void Update()
	{
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