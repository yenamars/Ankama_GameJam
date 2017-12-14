using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRelauch : MonoBehaviour 
{
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene s = SceneManager.GetActiveScene();
            SceneManager.LoadScene(s.name, LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
