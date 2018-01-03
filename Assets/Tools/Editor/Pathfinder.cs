using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using PathFinding;

public class Pathfinder : EditorWindow
{
	public List<string> AllTypes;
    private void OnEnable()
    {
		AllTypes = new List<string>();
		AllTypes.Add(WalkType.Walk.ToString());
		AllTypes.Add(WalkType.Fly.ToString());
    }

    private void OnDisable()
    {
    }
    [MenuItem("Pathfinder/Pathfinder")]
    private static void ShowWindow()
    {
		Pathfinder pathfinder = new Pathfinder();
		pathfinder.Show();
    }
	
	private void DisableEditor()
	{
		SceneView.onSceneGUIDelegate -= SceneGUI;
//		if (tileManager != null)
//		{
//			tileManager.m_selectedTile.Clear();
//			tileManager.generateMesh();
//			tileManager.m_drawEditMode = false;
//			EditorUtility.SetDirty(tileManager);
//		}
	}
    public void OnGUI()
	{
		if(pathfindManager == null)
		{
			pathfindManager = Component.FindObjectOfType<PathFinding.PathFindingManager>();
			return;

		}
		EditMode(); 
		GUILayout.BeginHorizontal();
		GUILayout.Label("Pathfinder:", GUILayout.Width(Screen.width/3 - 5));
		GameObject tilemanagerParent =
			EditorGUILayout.ObjectField(pathfindManager, typeof (GameObject), true, GUILayout.Width(Screen.width/3 - 5)) as
				GameObject;
		if (GUILayout.Button("reset", GUILayout.Width(Screen.width/3 - 5)))
		{
			Reset();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Area", GUILayout.Width(Screen.width/3 - 5));
		Rect OldRect = EditorGUILayout.RectField(pathfindManager.AreaRect, GUILayout.Width(2*Screen.width/3 - 5));
		if(OldRect != pathfindManager.AreaRect || pathfindManager.CompleteNodeArea == null)
		{
			pathfindManager.AreaRect = OldRect;
			RebuildArea();
		}
		DrawArea();
		GUILayout.EndHorizontal();

		EditSelection();
    }


	private void EditMode()
	{

		//if (pathfindManager == null) return;
		//pathfindManager.CompleteNodeArea.AllNodes = pathfindManager.CompleteArea;
		GUILayout.BeginHorizontal();
		bool newEdit = GUILayout.Toggle(pathfindManager.Editable, "Edit Mode");
		if (newEdit != pathfindManager.Editable)
		{
			if (newEdit)
			{

				m_selectecPoints = new List<PathfindNode>(); 
				SceneView.onSceneGUIDelegate += SceneGUI;
				m_moussePressed = false;
				Reset();
				//RebuildArea();
			}
			else
			{
				DisableEditor();
			}
			pathfindManager.Editable = newEdit;
			EditorUtility.SetDirty(pathfindManager);
		}

		GUILayout.EndHorizontal();
	}
	void EditSelection ()
	{
		EditGroupViewMode ();		
		bool needSave = false;
		
		SelectWalkType ();    

		pathfindManager.drawWalkType = (WalkType)m_curentTab;

		if(m_selectecPoints == null || m_selectecPoints.Count ==0)return;
		PathfindNode root = m_selectecPoints[0];

		PathfindNodeGroupParameter curentParameter = root.GetParameter(m_curentTab);
		EditNodeParameters (ref needSave, curentParameter);

	}

	void SelectWalkType ()
	{
		bool needSave = false;
		GUILayout.BeginHorizontal ();
		int newType = GUILayout.Toolbar (m_curentTab, AllTypes.ToArray ());
		needSave = newType != m_curentTab;
		m_curentTab = newType;
		GUILayout.EndHorizontal ();

		
		if(needSave)
		{
			EditorUtility.SetDirty(pathfindManager);
		}
	}
	void EditGroupViewMode ()
	{
		bool needSave = false;
		EditorGUILayout.BeginHorizontal ();
		bool oldEditable = Editable;
		Editable= EditorGUILayout.BeginToggleGroup ("Editable", Editable);
		if(Editable != oldEditable)
		{
			if(Editable)
			{
				SceneView.onSceneGUIDelegate += SceneGUI;
			}else
			{

				SceneView.onSceneGUIDelegate -= SceneGUI;
			}
		}
		if (EditorGUILayout.Toggle ("walks", m_toggleIndex == 0))
			m_toggleIndex = 0;
		if (EditorGUILayout.Toggle ("groups", m_toggleIndex == 1))
			m_toggleIndex = 1;
		EditorGUILayout.EndToggleGroup ();
		if (pathfindManager.drawWalks != (m_toggleIndex == 0)) 
		{
			needSave = true;
			pathfindManager.drawWalks = m_toggleIndex == 0;
			Repaint ();
		}
		EditorGUILayout.EndHorizontal ();

		if(needSave)
		{
			EditorUtility.SetDirty(pathfindManager);
		}
	}

	void EditNodeParameters (ref bool needSave, PathfindNodeGroupParameter curentParameter)
	{
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("_______________NODES______________:");
		if(m_selectecPoints.Count > 0)		
			GUILayout.Label("Pos: "+m_selectecPoints[0].Point.X+" : "+m_selectecPoints[0].Point.Y, GUILayout.Width(Screen.width/3 - 5));
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Group:", GUILayout.Width (Screen.width / 3 - 5));
		int newGroup = EditorGUILayout.IntField (curentParameter.Group, GUILayout.Width (Screen.width / 3 - 5));
		needSave |= curentParameter.Group != newGroup;
		if (needSave) {
			foreach (PathfindNode node in m_selectecPoints) {
				node.GetParameter (m_curentTab).Group = newGroup;
			}
		}
		curentParameter.Group = newGroup;
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("walkable:", GUILayout.Width (Screen.width / 3 - 5));
		bool walkable = EditorGUILayout.Toggle (curentParameter.Enable, GUILayout.Width (2 * Screen.width / 3 - 5));
		if (walkable != curentParameter.Enable) {
			foreach (PathfindNode node in m_selectecPoints) {
				needSave |= curentParameter.Enable != walkable;
				node.GetParameter (m_curentTab).Enable = walkable;
			}
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();		
		GUILayout.Label ("Bridges:", GUILayout.Width (Screen.width / 3 - 5));
		EditorGUILayout.BeginVertical();
		if(curentParameter.GroupBridge != null)
		{
			for(int i = 0;i < curentParameter.GroupBridge.Count;i++)
			{
				
				EditorGUILayout.BeginHorizontal();	
				int newGroupID = EditorGUILayout.IntField (curentParameter.GroupBridge[i], GUILayout.Width (Screen.width / 6 - 5));
				if(newGroupID != curentParameter.GroupBridge[i])
				{
					foreach(PathfindNode node in m_selectecPoints)
					{
						if(node.GetParameter(m_curentTab).GroupBridge.Count <= i)
							node.GetParameter(m_curentTab).GroupBridge.Add(newGroupID);
						node.GetParameter(m_curentTab).GroupBridge[i] = newGroupID;
					}
					needSave = true;
				}		
				if(GUILayout.Button("-", GUILayout.Width (Screen.width / 6 - 5)))
				{
					foreach(PathfindNode node in m_selectecPoints)
					{
						node.GetParameter(m_curentTab).GroupBridge.RemoveAt(i);
					}
					needSave = true;
				}
				EditorGUILayout.EndHorizontal();
					
			}
		}
		EditorGUILayout.EndVertical();
		if(GUILayout.Button("+", GUILayout.Width (Screen.width / 3 - 5)))
		{
			foreach(PathfindNode node in m_selectecPoints)
			{
				node.GetParameter(m_curentTab).GroupBridge.Add(0);
			}
			needSave = true;
		}
		EditorGUILayout.EndHorizontal();

	}

	void RebuildArea ()
	{
		//if(pathfindManager.CompleteArea == null)
		{
			pathfindManager.CompleteNodeArea = new NodesArea((int)pathfindManager.AreaRect.width,(int)pathfindManager.AreaRect.height);
			//pathfindManager.CompleteArea = new List<PathfindNode>();
		}
		pathfindManager.CompleteNodeArea.Clear();
		for(int i = 0;i<= pathfindManager.AreaRect.width;i++)
		{
			for(int j = 0;j<= pathfindManager.AreaRect.height;j++)
			{
				//pathfindManager.CompleteArea.Add(new PathfindNode(new Point( i,j)));
				pathfindManager.CompleteNodeArea.AddNode(new Point( i,j));
			}
		}
		EditorUtility.SetDirty(pathfindManager);
	}

	void Reset ()
	{
//		RebuildArea();
//		pathfindManager.CompleteNodeArea.AllNodes = pathfindManager.CompleteArea;
//		pathfindManager.CompleteNodeArea.BuildArea();

		foreach(PathfindNode node in pathfindManager.CompleteNodeArea.AllNodes)
		{
			node.Selected = false;
		}
	}

	void DrawArea ()
	{
		Rect AreaRect = pathfindManager.AreaRect;

	}
    private void SceneGUI(SceneView sceneView)
	{
		if( Event.current.type == EventType.Repaint)
		{
			return;
		}
		if(Event.current.type == EventType.Layout )
		{ 
			int controlID = GUIUtility.GetControlID( FocusType.Passive);
			HandleUtility.AddDefaultControl(controlID);
			return;
		}
		//Debug.Log(Event.current.type);
		if(Event.current.type == EventType.Used)
		{
		}
		if ( Event.current.isMouse)
		{
			if(m_moussePressed)
			{
				if(Event.current.type == EventType.MouseUp && Event.current.button == 0)
				{
					m_moussePressed = false;
					Repaint();
					//HandleUtility.AddDefaultControl( sceneView.GetInstanceID());
				}
				else
				{
					Reset();
					ComputeSelectSquare(sceneView);
				}
			}else
			{
				handleMouseEvent(sceneView);
			}				
		}
	}
	private void ComputeSelectSquare(SceneView sceneView)
	{
		if(Editable == false)
			return;	
		Vector3 mouseposition = Event.current.mousePosition;
		List<Vector3> screenPoints = new List<Vector3>();
		screenPoints.Add(mouseposition);
		screenPoints.Add(new Vector3(mouseposition.x,m_squareFirstPoint.y,0));
		screenPoints.Add(m_squareFirstPoint);
		screenPoints.Add(new Vector3(m_squareFirstPoint.x,mouseposition.y,0));
		
		var plane = new Plane(new Vector3(0,1,0), Vector3.zero);	
		List<Vector3> squareWorldPoint = new List<Vector3>();
		
		for(int i =0 ;i< screenPoints.Count;i++)
		{
			Ray ray = HandleUtility.GUIPointToWorldRay(screenPoints[i]);
			float enterPoint = 0;
			if (plane.Raycast(ray, out enterPoint))
			{
				squareWorldPoint.Add(ray.origin + ray.direction*enterPoint);
			}
		}
		//tileManager.cameraRotation= sceneView.camera.transform.eulerAngles.y;
		SelectTilesInSquare(squareWorldPoint);

		EditorUtility.SetDirty(pathfindManager);
	}
	void SelectTilesInSquare(List<Vector3> list)
	{
		if(pathfindManager == null)
			return;
		if(Event.current.control == false)
			m_selectecPoints.Clear();
		
		foreach(PathfindNode node in pathfindManager.CompleteNodeArea.AllNodes)
		{
			Vector3 tileCenter =   NawakTools.Rotate(NawakTools.cellToWorld(node.Point)
			                                                            ,(pathfindManager.transform.rotation.eulerAngles.y)*Mathf.PI/180);
			float sign = 0;
			bool AddTile = true;
			for(int i =0;i< list.Count;i++)
			{
				Vector3 v1 = list[i] - tileCenter;
				Vector3 v2 = list[(i+1)%list.Count] - tileCenter;
				float crossSign = Vector3.Cross(v1,v2).y;
				if(sign == 0)
				{
					sign = crossSign;
					continue;
				}
				if(sign*crossSign < 0)
				{
					AddTile = false;
				}
			}
			if(AddTile)
			{
				m_selectecPoints.Add(node);
			}
		}
		OnSelect();
	}

	private void handleMouseEvent(SceneView sceneView)
	{
		if(Editable == false)
			return;

		if(pathfindManager == null)
		{
			DisableEditor();
			return;
		}
			
		//ComputeSelectSquare(sceneView);
		bool validEvent = (Event.current.type == EventType.MouseDown ) && Event.current.button == 0;

		if(validEvent == false) return;

		m_moussePressed = true;
		m_squareFirstPoint = Event.current.mousePosition;

		NawakTools.s_tileSize = (int)pathfindManager.tileSize;

		Vector3 mouseposition = Event.current.mousePosition;
		var plane = new Plane(new Vector3(0,0,1), Vector3.zero);
		Ray ray = sceneView.camera.ScreenPointToRay(mouseposition);
		ray = HandleUtility.GUIPointToWorldRay(mouseposition);
		Vector3 worldPosition = Vector3.zero;
		float enterPoint = 0;
		if (plane.Raycast(ray, out enterPoint))
		{
			bool onAdding = false;
			worldPosition = ray.origin + ray.direction*enterPoint;
			if (Event.current.type == EventType.MouseMove && Event.current.modifiers == EventModifiers.Shift)
			{
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				Point selectedTile = null;
				PathfindNode selectedNode = null;
				if (Event.current.modifiers == EventModifiers.Alt)
				{
				}
				
				if (onAdding)
				{
				}
				else
				{
					//selectedTile = NawakTools.worldPositionToCellCenter(worldPosition);
					
					selectedTile	= new Point(new Vector2((int)(( -pathfindManager.transform.position.x +  worldPosition.x +1.25f/2)/(1.25f))
						,(int)( ( -pathfindManager.transform.position.y + worldPosition.y+1.25f/2)/(1.25f))));
					selectedNode = pathfindManager.GetNodeAt(selectedTile);
				}
				if (Event.current.modifiers != EventModifiers.Control && onAdding == false)
				{
					OnUnselect();
					m_selectecPoints.Clear();
				}
				if (selectedNode != null)
				{
					if(m_selectecPoints.Contains(selectedNode))
					{					
						OnUnselect();
						m_selectecPoints.Remove(selectedNode);
					}
					else
						m_selectecPoints.Add(selectedNode);
				}
				OnSelect();
				Repaint();
			}
			
			EditorUtility.SetDirty(pathfindManager);
		}
	}

	void OnSelect ()
	{
		if(m_selectecPoints.Count == 0) return;
		foreach(PathfindNode node in m_selectecPoints)
		{
			node.Selected = true;
		}
		//Reset();
		//RebuildArea ();
	}

	void OnUnselect ()
	{
		foreach(PathfindNode node in m_selectecPoints)
		{
			node.Selected = false;
		}
		Reset();
		//RebuildArea ();
	}
	private int m_toggleIndex;
	private int m_curentTab;
    private PathFinding.PathFindingManager pathfindManager;
	private List<PathfindNode> m_selectecPoints; 
	private bool m_moussePressed;
	private Vector3 m_squareFirstPoint;

	private bool Editable = true;
}