using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControler : MonoBehaviour
{


	[HideInInspector]
	public PlayerMoveControler Player;
	

	public static SceneControler Instance;
    private bool loading;
    private bool click;

	// Use this for initialization
	void Awake ()
	{
		Instance = this;
		DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene("Openning");
		//SceneManager.LoadScene("Common", LoadSceneMode.Additive);
        loading = false;
        click = false;;
	}

	public void StartGame()
	{
		StartCoroutine(StartGameCoroutine());
	}
	private IEnumerator StartGameCoroutine()
	{

		AsyncOperation loadOp = SceneManager.LoadSceneAsync("Loader", LoadSceneMode.Additive);
		while (loadOp.isDone == false)
		{
			yield return new WaitForEndOfFrame();
		}
		GameObject uiRoot = GameObject.FindGameObjectWithTag("UI");
		m_uiroot = uiRoot.GetComponent<UIRoot>();
		m_uiroot.group.alpha = 0;
		
		m_ChangerPanel = GameObject.FindGameObjectWithTag("LoaderPanel").GetComponent<SceneChanger>();
		m_ChangerPanel.GameOver.GetComponent<CanvasGroup>().alpha = 0;
		Player = GameObject.Instantiate(Resources.Load<PlayerMoveControler>("Player"));
		Player.transform.parent = Instance.transform;
		m_uiroot.InitialiseUI(Player);
		MoneyManager.instance.currentScore = 0;
		Player.SetAlive(false);
		
		//yield return new WaitForSeconds(10);
		LoadNextScene(0,"Title");
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
//
//        if (click == true && loading == false)
//        {
//            loading = true;
//            Seek();
//        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

//	public void Seek()
//	{
//		StartCoroutine(SeekCoroutine());
//	}

//	private IEnumerator SeekCoroutine()
//	{
//		//m_ChangerPanel.LoaderPanel.SetTrigger("Slide");
//		m_ChangerPanel.FaderText.text = "FLOOR " + (50- m_finshedLevelCount).ToString();
//		TweenAlpha.AddTween(m_ChangerPanel.Fader, 0, 1, 0.3f);
//		yield return new WaitForSeconds(00.3f);
//		SceneManager.LoadScene("Level_01", LoadSceneMode.Additive);
//		//SceneManager.UnloadSceneAsync("Splash");
//		Player.animator.SetTrigger("Out");
//		
//		Destroy(Splash);
//		yield return new WaitForSeconds(1.0f);
//		//m_ChangerPanel.LoaderPanel.SetTrigger("SlideOut");
//		TweenAlpha.AddTween(m_ChangerPanel.Fader, 1, 0, 0.3f);
//		//GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<Animator>().SetTrigger("Slide");
//		SceneRoot root = GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<SceneRoot>();
//		yield return new WaitForSeconds(00.5f);
//		Player.transform.position = root.PlayerSpawn.position;
//		Player.animator.SetTrigger("Fall");
//		yield return new WaitForSeconds(00.5f);
//		Player.SetAlive(true);
//		yield return new WaitForSeconds(00.5f);
//		root.StartScene(2);
//	}
	public void LoadNextScene(int levelID,string previousScene)
	{
		m_previousSceneName = previousScene;
		m_currentSceneID = levelID;
		StartCoroutine(LoadNextSceneCoroutine(0));
	}
	public void LoadNextScene()
	{
		LoadNextRoutine = StartCoroutine(LoadNextSceneCoroutine());
	}

	private Coroutine LoadNextRoutine;

	private IEnumerator LoadNextSceneCoroutine(float delay = 2)
	{
		
		m_ChangerPanel.FaderText.text = "FLOOR " + ( m_finshedLevelCount).ToString();
		m_finshedLevelCount++;
		yield return new WaitForSeconds(delay);
		Player.SetAlive(false);
		//m_ChangerPanel.LoaderPanel.SetTrigger("Slide");
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 0, 1, 0.3f);
		yield return new WaitForSeconds(0.75f);
		m_uiroot.group.alpha = 1;
		AsyncOperation unload = SceneManager.UnloadSceneAsync(m_previousSceneName);
		m_currentSceneID = m_currentSceneID + 1;
		if (m_currentSceneID > 7)
			m_currentSceneID = 3;
		m_currentDifficulty = m_currentDifficulty + 1;
		m_previousSceneName = "Level_" + m_currentSceneID.ToString("00");

		while (unload.isDone == false)
		{
			yield return new WaitForEndOfFrame();
		}
		//yield return new WaitForSeconds(1.0f);
		SceneManager.LoadScene(m_previousSceneName,LoadSceneMode.Additive);
		
		Player.animator.SetTrigger("Out");
		yield return new WaitForSeconds(1.0f);
		//Destroy(Splash);
		yield return new WaitForSeconds(0.3f);
		m_root = null;
		while (m_root == null)
		{
			m_root = GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<SceneRoot>();
			//yield return new WaitForEndOfFrame();
			yield return new WaitForSeconds(1.0f);
		}
		Player.transform.position = m_root.PlayerSpawn.position;
		//m_ChangerPanel.LoaderPanel.SetTrigger("SlideOut");
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 1, 0, 0.3f);
		//GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<Animator>().SetTrigger("Slide");
		yield return new WaitForSeconds(00.3f);
		
		Player.animator.SetTrigger("Fall");
		yield return new WaitForSeconds(00.5f);
		Player.SetAlive(true);
		yield return new WaitForSeconds(00.5f);
		m_root.StartScene(m_currentDifficulty);
		LoadNextRoutine = null;
	}
	
	public void OnGameOver()
	{
		m_root.StopMobs();
		if(LoadNextRoutine != null)
			StopCoroutine(LoadNextRoutine);
		StopCoroutine("LoadNextSceneCoroutine");
	}

	public void Reload()
	{
        m_waitForRestart = true;
		StartCoroutine(ReloadRoutine());
	}

	private IEnumerator ReloadRoutine()
	{
		
		TweenAlpha.AddTween(m_ChangerPanel.GameOver.gameObject, 0, 1, 0.3f);
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 0, 1, 0.3f);

        yield return new WaitForSeconds(2.0f);

        while (click == false)
        {
            yield return null;
        }
		GameObject.Destroy(Player.gameObject);
		m_currentDifficulty = 0;
		m_finshedLevelCount = 0;
		#if DEMO
		SceneManager.LoadScene("Openning");
	#else
		
		SceneManager.LoadScene("Title");
		#endif
		
	}

	private String m_previousSceneName;
	
	private SceneChanger m_ChangerPanel;
	private int m_currentSceneID = 1;
	private int m_currentDifficulty = 2;

	private int m_finshedLevelCount = 0;
    private bool m_waitForRestart = false;

	private UIRoot m_uiroot;
	private SceneRoot m_root;

}
