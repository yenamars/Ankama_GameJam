using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
    [SerializeField] private Camera cam;
    [SerializeField] private float cameraSmooth;
    [SerializeField] private Vector2 terrainLimitX;
    [SerializeField] private Vector2 terrainLimitY;

    private Transform trsf;
    private Transform playerTransform;
    private Vector2 cameraSize;

	void Awake () 
    {
        trsf = transform;
       // playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cameraSize.y = (float)cam.orthographicSize;
        cameraSize.x = (float)cam.orthographicSize * ((float)Screen.width / (float)Screen.height);
	}

	void Update () 
    {
	    if (playerTransform == null)
	    {
		    GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		    if(playerGO == null)
			    return;
		    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		    return;
	    }


	    Vector3 newPosition = Vector3.Lerp(trsf.localPosition, playerTransform.localPosition, Time.deltaTime * cameraSmooth);
        newPosition.x = Mathf.Clamp(newPosition.x, terrainLimitX.x + cameraSize.x, terrainLimitX.y - cameraSize.x);
        newPosition.y = Mathf.Clamp(newPosition.y, terrainLimitY.x + cameraSize.y, terrainLimitY.y - cameraSize.y);
        trsf.localPosition = newPosition;
	}
}
