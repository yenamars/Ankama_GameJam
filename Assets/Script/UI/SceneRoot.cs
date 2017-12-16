
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour
{
    public int difficulty = 5;
	public GameObject MobeRoot;
	public LevelState State;
	public Transform PlayerSpawn;
	[Header("Spawner")] 
	
	public LevelType  Type;

	public GameObject TraderPrefab;
	public GameObject ShooterPrefab;
	public GameObject DonaldPrefab;
	public GameObject SpawnerRoot;

	public GameObject MiniSpawerRoot;

	public void Awake()
	{
		MobeRoot.SetActive(false);
        spawners = GetComponentsInChildren<Spawner>();
	}
	
	public void Start()
	{
		if (SceneControler.Instance == null)
			AutoStar();
	}
	
	public void AutoStar()
	{
		
		SceneManager.LoadScene("Common", LoadSceneMode.Additive);
        player = GameObject.Instantiate(Resources.Load<PlayerMoveControler>("Player"));
		player.SetAlive(true);
		player.transform.position = PlayerSpawn.transform.position;
        StartScene(difficulty);
	}
    private PlayerMoveControler player;
	public void StartScene(int difficulty)
	{
        if (player == null)
            player = SceneControler.Instance.Player;
		if (Type == LevelType.Random)
		{
            List<Vector3> allSpawnPos = new List<Vector3>();
            for (int i = 0; i < SpawnerRoot.transform.childCount; i++)
            {
                allSpawnPos.Add(SpawnerRoot.transform.GetChild(i).position);
            }
			for (int i = 0; i < difficulty / 2; i++)
			{
                int index = (int) (allSpawnPos.Count * Random.value);

                if (allSpawnPos.Count == 0)
                    break;
                
                Vector3 spawnPos = allSpawnPos[index];
				GameObject PrefadToInstanciate = TraderPrefab;
				if (difficulty > 4)
					PrefadToInstanciate = Random.value > 0.5f ? ShooterPrefab : TraderPrefab;
				if (difficulty > 8 && i == 0)
					PrefadToInstanciate = DonaldPrefab;
				Instantiate(PrefadToInstanciate, MobeRoot.transform).transform.position = spawnPos;
				
                allSpawnPos.RemoveAt(index);
			}

			foreach (Spawner spawner in MiniSpawerRoot.GetComponentsInChildren<Spawner>())
			{
                difficulty = Mathf.Min(difficulty, 9);
				spawner.randomDelay = new Vector2(8.0f - difficulty*0.5f, 24 - difficulty * 2.0f);
                spawner.StartSpawn();
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

        if (player.IsAlive() == false)
            return;
        
		int aliveCount = 0;
		foreach (BaseAI ai in m_BaseMobs)
		{
			aliveCount += ai.IsDead ? 0 : 1;
		}
            
        bool spawnerMobsAreAlive = false;

        if (aliveCount == 0 && MiniSpawerRoot != null)
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].Stop();

                if (spawners[i].aliveMobs > 0)
                {
                    spawnerMobsAreAlive = true;
                }
            }
        }

        if (spawnerMobsAreAlive == false && aliveCount == 0 && State == LevelState.Play) 
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
    private Spawner[] spawners;
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