
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour
{


	public GameObject MobeRoot;
	public LevelState State;
	public Transform PlayerSpawn;
	[Header("Spawner")] 
	
	public LevelType  Type;

	public GameObject TraderPrefab;
	public GameObject SpawnerRoot;

	public GameObject MiniSpawerRoot;

	public void Awake()
	{
		MobeRoot.SetActive(false);
	}
	
	public void Start()
	{
		if (SceneControler.Instance == null)
			AutoStar();
	}
	
	public void AutoStar()
	{
		
		SceneManager.LoadScene("Common", LoadSceneMode.Additive);
		GameObject.Instantiate(Resources.Load<PlayerMoveControler>("Player")).SetAlive(true);
		StartScene(4);
	}
	
	public void StartScene(int difficulty)
	{
		if (Type == LevelType.Random)
		{
			for (int i = 0; i < difficulty; i++)
			{
				int index = (int) (SpawnerRoot.transform.childCount * Random.value);
				Transform spawnPos = SpawnerRoot.transform.GetChild(index);
				Instantiate(TraderPrefab, spawnPos.position, Quaternion.identity);
				Destroy(SpawnerRoot.transform.GetChild(index).gameObject);
			}

			foreach (Spawner spawner in MiniSpawerRoot.GetComponents<Spawner>())
			{
				spawner.randomDelay = new Vector2(1.5f - difficulty*0.1f,3* difficulty*0.2f);
			}
		}
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

public enum LevelType
{
	Predef,
	Random
}

public enum LevelState
{
	Intro,
	Play,
	Outro
}