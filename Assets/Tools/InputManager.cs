using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {


	public List<FingerInput> Inputs;

	public static InputManager instance
	{
		get{
			return s_instance;
		}
	}

	public bool Drag
	{
		get { return Inputs[0].Drag; }
	}
	public bool Pressed
	{
		get { return Inputs[0].Pressed; }
	}
	public bool Released
	{
		get { return Inputs[0].Released; }
	}
	public float PressedTime
	{
		get { return Inputs[0].PressedTime; }
	}

	public Vector3 mousePosition
	{
		get { return Inputs[0].MouseScreenPosition; }
	}

	// Use this for initialization
	void Start () {
		s_instance = this;
		Inputs = new List<FingerInput> ();
		Inputs.Add (new FingerInput ());
		Inputs.Add (new FingerInput ());
	}
	
	// Update is called once per frame
	void Update()
	{

#if !UNITY_EDITOR  && (UNITY_IOS || UNITY_ANDROID)
			//if (Input.touchCount > 0)
			for (var i = 0; i < Inputs.Count; i++)
			{
				var input = Inputs[i];
				if(input.Down == false)
					SetMousePresse(i,false);
			}
			for(int i =0 ;i< Input.touchCount;i++)	
			{
				
				Touch touch = Input.GetTouch(i);
				switch (touch.phase)
				{
					case TouchPhase.Began:
	
						OnTouchBegan(touch);
						break;
					case  TouchPhase.Canceled :
					case  TouchPhase.Ended :
						OnTouchContinues(touch, touch.phase);
						
						break;
					default:
						OnTouchContinues(touch, touch.phase);
						break;
				}
			}
		
	
		#else


		bool mousePressed_Left = Input.GetMouseButton(0);
		SetMousePresse (0, mousePressed_Left);
		
		//if (SingleStick == false)
		{
			bool mousePressed_Right = Input.GetMouseButton(1);
			SetMousePresse( 1, mousePressed_Right);
		}
		#endif

	}

	private void OnTouchBegan(Touch touch)
	{
		while (Inputs.Count <= touch.fingerId)
		{
			Inputs.Add(new FingerInput());
		}
		
		Inputs[touch.fingerId].FingerID = touch.fingerId;
		SetMousePresse( touch.fingerId, true);
		
	}

	private void OnTouchContinues(Touch touch,TouchPhase phase)
	{
		for (int i = 0;i < Inputs.Count;i++) 
		{
			FingerInput input = Inputs[i];
			if (input.FingerID == touch.fingerId)
			{
				SetMousePresse(touch.fingerId, phase != TouchPhase.Ended && phase != TouchPhase.Canceled);
			}
		}
	}

	private void SetMousePresse(int fingerIndex,bool pressed)
	{
		if(pressed)	
		{
			if(Inputs[fingerIndex].PreviouslyPressed)
			{
				Inputs[fingerIndex].UpdateDraging();
			}else
			{
				Inputs[fingerIndex].OnPress(fingerIndex);
			}
			Vector2 screenPosition = Input.mousePosition;
			
			
			
#if !UNITY_EDITOR  && (UNITY_IOS || UNITY_ANDROID)
			for(int i =0 ;i< Input.touchCount;i++)	
			{
				Touch touch = Input.GetTouch(i);
				if (touch.fingerId == fingerIndex)
				{
					screenPosition = touch.position;
				}
			}	
	
			#endif
			Inputs[fingerIndex].MouseScreenPosition = screenPosition;// Input.mousePosition;
			Inputs[fingerIndex].MouseWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);;//Input.mousePosition);
			Inputs[fingerIndex].ViewportPosition = Camera.main.ScreenToViewportPoint(screenPosition);;//Input.mousePosition);
			
		}else
		{
			
			if(Inputs[fingerIndex].PreviouslyPressed)
			{
				Inputs[fingerIndex].OnRelease();
			}else
			{
				Inputs[fingerIndex].Released = false;
			}
		}
		Inputs[fingerIndex].PreviouslyPressed = pressed;
	}



	static InputManager s_instance;
}

public class FingerInput
{
	public int FingerID = -1;

	public bool PreviouslyPressed;
	public bool Pressed;
	public bool Drag;
	public bool Released;
	public bool Down;

	public Vector3 MouseScreenPosition;

	public Vector3 ViewportPosition;
	public Vector3 ViewportPressedPosition;
	
	public Vector3 MouseWorldPosition;
	public Vector3 MouseWorldPressPosition;
	public float PressedTime;

	public Vector3 DragDirection()
	{
		return MouseWorldPosition - MouseWorldPressPosition;
	}

	public void OnRelease ()
	{
		Pressed = false;
		Drag = false;
		Released = true;
		FingerID = -1;
		Down = false;
	}

	private bool m_pressedGameObjec;
	void CheckUIClic()
	{
		m_pressedGameObjec = false;
		PointerEventData cursor = new PointerEventData(EventSystem.current); 
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		foreach (RaycastResult hit in objectsHit)
		{
			//Debug.Log( hit.gameObject.name );
			//Debug.Log( LayerMask.LayerToName(hit.gameObject.layer) );
			m_pressedGameObjec |= (hit.gameObject.layer == LayerMask.NameToLayer("UI"));
		}
	}
	public void OnPress (int index)
	{
		CheckUIClic();
		if(m_pressedGameObjec)
			return;
		
		
		MouseScreenPosition = Input.mousePosition;
//		Debug.Log(MouseScreenPosition);
		MouseWorldPressPosition= Camera.main.ScreenToWorldPoint( Input.mousePosition);
		ViewportPressedPosition = Camera.main.ScreenToViewportPoint( Input.mousePosition);
		
		#if !UNITY_EDITOR  && (UNITY_IOS || UNITY_ANDROID)
		MouseWorldPressPosition=Camera.main.ScreenToWorldPoint( Input.GetTouch(index).position);
		ViewportPressedPosition=Camera.main.ScreenToViewportPoint( Input.GetTouch(index).position);
		#endif
		Pressed = true;
		Down = true;
		Drag = false;
		Released = false;
		PressedTime = Time.realtimeSinceStartup;
	}

	public void UpdateDraging ()
	{
		Pressed = false;
		Down = true;
		Drag = (ViewportPosition - ViewportPressedPosition).magnitude > 0.05f;
		Released = false;
	}
}


