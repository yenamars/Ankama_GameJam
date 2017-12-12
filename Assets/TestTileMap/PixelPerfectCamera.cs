using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour 
{
    [SerializeField] private int pixelsPerUnit;
    private Camera cam;
    private int previousResolution;

	void Awake () 
    {
        cam = GetComponent<Camera>();
        previousResolution = Screen.height;
        UpdateCameraSize();
	}
	
	void Update () 
    {
        if (previousResolution != Screen.height)
        {
            UpdateCameraSize();
            previousResolution = Screen.height;
        }
	}

    void UpdateCameraSize()
    {
        cam.orthographicSize = (float)Screen.height / ((float)pixelsPerUnit * 2.0f);
    }
}
