using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControler : MonoBehaviour
{


	public PlayerMoveControler Player;
	public GameObject Splash;

	public static SceneControler Instance;
	
	// Use this for initialization
	void Awake ()
	{
		Instance = this;
		DontDestroyOnLoad(gameObject);
		Player.SetAlive(false);
		SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
		SceneManager.LoadScene("Common", LoadSceneMode.Additive);
	}

	public void Start()
	{
		m_ChangerPanel = GameObject.FindGameObjectWithTag("LoaderPanel").GetComponent<SceneChanger>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Seek()
	{
		StartCoroutine(SeekCoroutine());
		
	}

	private IEnumerator SeekCoroutine()
	{
		//m_ChangerPanel.LoaderPanel.SetTrigger("Slide");
		m_ChangerPanel.FaderText.text = "FLOOR " + (50- m_finshedLevelCount).ToString();
		m_ChangerPanel.GameOver.color = new Color(1,1,1,0);
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 0, 1, 0.3f);
		yield return new WaitForSeconds(00.3f);
		SceneManager.LoadScene("s02",LoadSceneMode.Additive);
		//SceneManager.UnloadSceneAsync("Splash");
		Player.animator.SetTrigger("Out");
		
		Destroy(Splash);
		yield return new WaitForSeconds(1.0f);
		//m_ChangerPanel.LoaderPanel.SetTrigger("SlideOut");
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 1, 0, 0.3f);
		//GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<Animator>().SetTrigger("Slide");
		SceneRoot root = GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<SceneRoot>();
		yield return new WaitForSeconds(00.5f);
		Player.transform.position = root.PlayerSpawn.position;
		Player.animator.SetTrigger("Fall");
		yield return new WaitForSeconds(00.5f);
		Player.SetAlive(true);
		yield return new WaitForSeconds(00.5f);
		root.StartScene(2);
	}

	public void LoadNextScene()
	{
		StartCoroutine(LoadNextSceneCoroutine());
	}

	private IEnumerator LoadNextSceneCoroutine()
	{
		
		yield return new WaitForSeconds(01.0f);
		m_finshedLevelCount++;
		m_ChangerPanel.FaderText.text = "FLOOR " + (50- m_finshedLevelCount).ToString();
		yield return new WaitForSeconds(01.0f);
		Player.SetAlive(false);
		//m_ChangerPanel.LoaderPanel.SetTrigger("Slide");
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 0, 1, 0.3f);
		yield return new WaitForSeconds(1.0f);
		string sceneName = "s" + string.Format("{0:D2}", m_currentSceneID);
		SceneManager.UnloadSceneAsync(sceneName);
		m_currentSceneID = m_currentSceneID + 1;
		if (m_currentSceneID > 7)
			m_currentSceneID = 4;
		m_currentDifficulty = m_currentDifficulty + 1;
		sceneName = "s" + string.Format("{0:D2}", m_currentSceneID);
		SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
		//SceneManager.UnloadSceneAsync("Splash");
		Player.animator.SetTrigger("Out");
		
		//Destroy(Splash);
		yield return new WaitForSeconds(0.3f);
		SceneRoot root = GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<SceneRoot>();
		Player.transform.position = root.PlayerSpawn.position;
		//m_ChangerPanel.LoaderPanel.SetTrigger("SlideOut");
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 1, 0, 0.3f);
		//GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<Animator>().SetTrigger("Slide");
		yield return new WaitForSeconds(00.3f);
		
		Player.animator.SetTrigger("Fall");
		yield return new WaitForSeconds(00.5f);
		Player.SetAlive(true);
		yield return new WaitForSeconds(00.5f);
		root.StartScene(m_currentDifficulty);
	}
	
	private SceneChanger m_ChangerPanel;
	private int m_currentSceneID = 2;
	private int m_currentDifficulty = 2;

	private int m_finshedLevelCount = 0;

	public void Reload()
	{
		StartCoroutine(ReloadRoutine());
	}

	private IEnumerator ReloadRoutine()
	{
		
		TweenAlpha.AddTween(m_ChangerPanel.GameOver.gameObject, 0, 1, 0.3f);
		TweenAlpha.AddTween(m_ChangerPanel.Fader, 0, 1, 0.3f);
		yield return new WaitForSeconds(02.0f);
		//m_ChangerPanel.LoaderPanel.SetTrigger("Slide");
		//yield return new WaitForSeconds(0.3f);
		
		SceneManager.LoadScene("Openning");
	}
}
