using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour 
{
    public Transform imageTransform;

	void Awake () 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
	}
	
	void Update () 
    {
        Vector3 mp = Input.mousePosition;
        imageTransform.position = new Vector3(mp.x, mp.y, 0.0f);
	}
}
