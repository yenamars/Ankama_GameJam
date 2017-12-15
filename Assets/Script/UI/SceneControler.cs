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
		m_ChangerPanel.LoaderPanel.SetTrigger("Slide");
		yield return new WaitForSeconds(00.3f);
		SceneManager.LoadScene("s02",LoadSceneMode.Additive);
		//SceneManager.UnloadSceneAsync("Splash");
		Player.animator.SetTrigger("Out");
		
		Destroy(Splash);
		yield return new WaitForSeconds(0.3f);
		m_ChangerPanel.LoaderPanel.SetTrigger("SlideOut");
		//GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<Animator>().SetTrigger("Slide");
		yield return new WaitForSeconds(01.0f);
		
		Player.animator.SetTrigger("Fall");
		yield return new WaitForSeconds(00.5f);
		Player.SetAlive(true);
		yield return new WaitForSeconds(00.5f);
		GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<SceneRoot>().StartScene();
	}

	public void LoadNextScene()
	{
		StartCoroutine(LoadNextSceneCoroutine());
	}

	private IEnumerator LoadNextSceneCoroutine()
	{
		m_ChangerPanel.LoaderPanel.SetTrigger("Slide");
		yield return new WaitForSeconds(00.3f);
		string sceneName = "s" + string.Format("{0:D2}", m_currentSceneID);
		SceneManager.UnloadSceneAsync(sceneName);
		m_currentSceneID++;
		sceneName = "s" + string.Format("{0:D2}", m_currentSceneID);
		SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
		//SceneManager.UnloadSceneAsync("Splash");
		Player.animator.SetTrigger("Out");
		
		//Destroy(Splash);
		yield return new WaitForSeconds(0.3f);
		m_ChangerPanel.LoaderPanel.SetTrigger("SlideOut");
		//GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<Animator>().SetTrigger("Slide");
		yield return new WaitForSeconds(01.0f);
		
		Player.animator.SetTrigger("Fall");
		yield return new WaitForSeconds(00.5f);
		Player.SetAlive(true);
		yield return new WaitForSeconds(00.5f);
		GameObject.FindGameObjectWithTag("SceneRoot").GetComponent<SceneRoot>().StartScene();
	}
	
	private SceneChanger m_ChangerPanel;
	private int m_currentSceneID = 2;
}
