
using System.Collections.Generic;
using System.Linq;
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
	public GameObject ShooterPrefab;
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
		PlayerMoveControler player = GameObject.Instantiate(Resources.Load<PlayerMoveControler>("Player"));
		player.SetAlive(true);
		player.transform.position = PlayerSpawn.transform.position;
		StartScene(10);
	}
	
	public void StartScene(int difficulty)
	{
		if (Type == LevelType.Random)
		{
			for (int i = 0; i < difficulty; i++)
			{
				int index = (int) (SpawnerRoot.transform.childCount * Random.value);
				Transform spawnPos = SpawnerRoot.transform.GetChild(index);
				GameObject PrefadToInstanciate = TraderPrefab;
				if (difficulty > 5)
					PrefadToInstanciate = Random.value > 0.5f ? ShooterPrefab : TraderPrefab;
				Instantiate(PrefadToInstanciate, MobeRoot.transform).transform.position = spawnPos.position;
				
				Destroy(SpawnerRoot.transform.GetChild(index).gameObject);
			}

			foreach (Spawner spawner in MiniSpawerRoot.GetComponentsInChildren<Spawner>())
			{
				spawner.MobRoot = MobeRoot.transform;
				spawner.randomDelay = new Vector2(10.0f - difficulty*0.5f,20- difficulty);
			}
		}
		MobeRoot.SetActive(true);

		m_BaseMobs = MobeRoot.GetComponentsInChildren<BaseAI>().ToList();
		State = LevelState.Play;
	}

	public void Update()
	{
		if(State != LevelState.Play)
			return;
		int aliveCount = 0;
		foreach (BaseAI ai in m_BaseMobs)
		{
			aliveCount+= ai.IsDead ? 0 : 1;
		}
		if (aliveCount == 0 && MiniSpawerRoot != null)
		{
			foreach (Spawner spawner in MiniSpawerRoot.GetComponentsInChildren<Spawner>())
			{
				spawner.Stop();
			}
		}
		
		foreach (BaseAI ai in MobeRoot.GetComponentsInChildren<BaseAI>())
		{
			aliveCount += ai.IsDead ? 0 : 1;
		}
		if (aliveCount == 0 && State == LevelState.Play) 
		{
			State = LevelState.Outro;
			OnSceneClear();
		}
	}

	public void OnSceneClear()
	{
		SceneControler.Instance.LoadNextScene();
	}

	private List<BaseAI> m_BaseMobs;

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