using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PathFinding
{
public class PathFindingManager : MonoBehaviour {

		public static PathFindNeighbourg[] Neighbourhood ;
		[HideInInspector]
		public NodesArea CompleteNodeArea;
		public List<PathfindNode> CompleteArea;
//		public List<PathfindNode> WalkableArea;
//		public Dictionary<Point,PathfindNode> WalkableAreaDico;

		public float tileSize;

		public Color WalkableColor;
		public Color FlyableColor;
		public Color NotWalkableColor;
		public Color NotFlyableColor;

		[HideInInspector]
		public Rect AreaRect;
		[HideInInspector]
		public bool Editable;
		[HideInInspector]
		[NonSerialized]
		public bool drawWalks = true;
		public WalkType drawWalkType;
		
		[HideInInspector]
		[NonSerialized]
		public List<PathfindNode> Bridges;

		public static PathFindingManager instance
		{
				get{
				return GameObject.FindGameObjectWithTag("Pathfinder").GetComponent<PathFindingManager>();
//					if(s_instance == null)
//						Debug.LogError("No PathFinder instance set");
//					return s_instance;
			}
		}
		//public static PathFindingManager s_instance;

		public delegate void PathComplete(Path path);

		void OnDrawGizmos()
		{
			if(CompleteNodeArea == null || CompleteNodeArea.AllNodes == null)
				return;
			if(Editable == false)return;
			//CompleteNodeArea.AllNodes = CompleteArea;
//			if(CompleteNodeArea.AllNodes == null )
//				CompleteNodeArea.AllNodes = CompleteArea;
			for (int i = 0; i < CompleteNodeArea.AllNodes.Count; i++)
			 {
				PathfindNode node = CompleteNodeArea.AllNodes [i];
				Color GizmoColor = WalkableColor;
				if (drawWalks)
					GizmoColor = GetColorWalk (node);
				else
					GizmoColor = GetColorGroup (node);
				Gizmos.color = GizmoColor;
				// node.Walkable?WalkableColor:NotWalkableColor;
				float size = node.Selected ? 2 : 1.0f;
				Gizmos.DrawCube (new Vector3 (node.Point.X, node.Point.Y,0) *  tileSize + transform.position, Vector3.one * size*0.5f);
			}
//			for(int i = 0;i<= AreaRect.width;i++)
//			{
//				for(int j = 0;j<= AreaRect.height;j++)
//				{
//					Gizmos.DrawCube(new Vector3(i+(int)AreaRect.x,0,j+(int)AreaRect.y) *2* tileSize,Vector3.one);
//				}
//			}
		}

		Color GetColorGroup (PathfindNode node)
		{
			Color GizmoColor = Color.white;
			GizmoColor.a = 0;
			PathfindNodeGroupParameter param = node.GetParameter (drawWalkType);
			if (param.Enable) 
			{
				GizmoColor = GetColorFromGoupID (param.Group);	
				if(param.GroupBridge.Count > 0)
					GizmoColor = Color.white;
				
			}
			return GizmoColor;
		}

		Color GetColorFromGoupID (int groupID)
		{
			Color GizmoColor = Color.white;
			if (groupID == 0)
				GizmoColor = WalkableColor;
			if (groupID == 1)
				GizmoColor = FlyableColor;
			if (groupID == 2)
				GizmoColor = NotFlyableColor;
			if (groupID == 3)
				GizmoColor = NotWalkableColor;
			return GizmoColor;
		}

		Color GetColorWalk (PathfindNode node)
		{
			Color GizmoColor = NotWalkableColor;
			PathfindNodeGroupParameter param = node.GetParameter (drawWalkType);
			if (param.Enable) 
			{
				GizmoColor = WalkableColor;
			}
			return GizmoColor;

		}

	public void Awake()
	{
//			for(int i = CompleteArea.Count-1;i>=0;i--)
//			{
//				if(CompleteArea[i].Walkable == false)
//					CompleteArea.RemoveAt(i);
//			}
			CompleteNodeArea.BuildArea();
//			WalkableArea = new List<PathfindNode>();
//			WalkableAreaDico = new Dictionary<Point,PathfindNode>();
			Bridges = new List<PathfindNode>();
			for(int i = CompleteNodeArea.AllNodes.Count-1;i>=0;i--)
			{
//				if(CompleteNodeArea.AllNodes[i].GetParameter(WalkType.Walk).Enable)
//				{
//					WalkableArea.Add(CompleteNodeArea.AllNodes[i]);
//					WalkableAreaDico.Add(CompleteNodeArea.AllNodes[i].Point,CompleteNodeArea.AllNodes[i]);
//				}
					
				foreach(PathfindNodeGroupParameter paramGroup in CompleteNodeArea.AllNodes[i].listParameters)
				{
					if(paramGroup.GroupBridge.Count > 0)
					{
						Bridges.Add(CompleteNodeArea.AllNodes[i]);
						continue;
					}
				}
				
			}
			//s_instance = this;
			List<PathFindNeighbourg> neighList = new List<PathFindNeighbourg>();
			neighList.Add(new PathFindNeighbourg(new Point(-1,0)));
			neighList.Add(new PathFindNeighbourg(new Point(1,0)));
			neighList.Add(new PathFindNeighbourg(new Point(0,1)));
			neighList.Add(new PathFindNeighbourg(new Point(0,-1)));
			
			

			Neighbourhood  = neighList.ToArray();		
			
		}
//		private void PrecalcultateBridges()
//		{
//			for(int i = 0;i < Bridges.Count;i++)
//			{
//				foreach(WalkType walkType in Enum.GetValues(typeof(WalkType)).Cast<WalkType>().ToList())
//				{					
//					for(int j = i;j < Bridges.Count;j++)
//					{
//						if(Bridges[i].GetParameter(walkType).GroupBridge.Count > 0 && Bridges[j].GetParameter(walkType).GroupBridge.Count > 0 )
//						{
//							AStartPathFinding(Bridges[i].Point,Bridges[j].Point);
//						}
//					}
//				}
//			}
//		}
		
		public bool HasBridgeBetween(PathfindNode startPoint,PathfindNode endPoint,WalkType walkType)
		{
//			if(m_linkedGroup == null)
//				m_linkedGroup = new Dictionary<int, List<int>>();
//				
//			if(m_linkedGroup.ContainsKey(startPoint.GetParameter(walkType).Group) )
//				return m_linkedGroup[startPoint.GetParameter(walkType).Group].Contains(endPoint.GetParameter(walkType).Group);

			if(m_linkedGroupInfo == null)
				m_linkedGroupInfo = new Dictionary<KeyValuePair<int,int>, bool>();

			KeyValuePair <int,int> pair = new KeyValuePair<int,int>(startPoint.GetParameter(walkType).Group,endPoint.GetParameter(walkType).Group) ;
			if(m_linkedGroupInfo.ContainsKey(pair) )
			{
				//Debug.Log("pair in dico");
				return m_linkedGroupInfo[pair];
			}

			List<int> walkedGroup = new List<int>();
			List<PathfindNode> intermediates = new List<PathfindNode>();
			intermediates.Add(startPoint);
			List<int> linkedGroups = new List<int>();
			linkedGroups.Add(startPoint.GetParameter(walkType).Group);
			bool hasLink = HasLinkBetween(startPoint.GetParameter(walkType).Group,endPoint.GetParameter(walkType).Group,walkType,ref linkedGroups);
//			if(m_linkedGroup.ContainsKey(startPoint.GetParameter(walkType).Group) == false)
//				m_linkedGroup.Add(startPoint.GetParameter(walkType).Group,linkedGroups);
//			else
//				m_linkedGroup[startPoint.GetParameter(walkType).Group] = linkedGroups;
			if(m_linkedGroupInfo.ContainsKey(pair) == false)
				m_linkedGroupInfo.Add(pair,hasLink);
			else
				m_linkedGroupInfo[pair] = hasLink;
			return hasLink;
//			return GetIntermediatesPoint (startPoint.GetParameter(walkType).Group, 
//			                              endPoint.GetParameter(walkType).Group, walkType,ref walkedGroup,ref intermediates,endPoint);
		}
		//private Dictionary<int,List<int>> m_linkedGroup;
		private Dictionary<KeyValuePair<int,int>,bool> m_linkedGroupInfo; 

		private bool HasLinkBetween(int startGroup,int endGroup,WalkType walkType,ref List<int> linkedGroups)
		{
			bool HasNewGroups = false;
			for (int bri = instance.Bridges.Count-1; bri >=0; bri--) 
			{
				PathfindNode bridge = instance.Bridges [bri];
				PathfindNodeGroupParameter groupParameter = bridge.GetParameter (walkType);
				bool containsPreviousGroup = linkedGroups.Contains(groupParameter.Group);
				for (int i = 0; i < groupParameter.GroupBridge.Count; i++) 
				{
					int bridgeGroup = groupParameter.GroupBridge [i];
					if (linkedGroups.Contains(bridgeGroup)) 
					{
						containsPreviousGroup = true;
					}
				}
				
				if(containsPreviousGroup)
				{
					
					if(linkedGroups.Contains(groupParameter.Group)==false)
					{						
						HasNewGroups = true;
						linkedGroups.Add(groupParameter.Group);
					}
					for (int i = 0; i < groupParameter.GroupBridge.Count; i++) 
					{
						int bridgeGroup = groupParameter.GroupBridge [i];
						if(linkedGroups.Contains(bridgeGroup)==false)
						{
							HasNewGroups = true;
							linkedGroups.Add(bridgeGroup);
						}
					}
				}
			}
			if(HasNewGroups)
				return HasLinkBetween(startGroup,endGroup,walkType,ref linkedGroups);
			else
				return linkedGroups.Contains(endGroup);
			return false;
		}
		
		public NodesArea GetAllowedNode (List<WalkType> walksTypes)
		{
			NodesArea AllowArea = new NodesArea(CompleteNodeArea.Width,CompleteNodeArea.Height);
			for(int i =0 ;i< CompleteNodeArea.AllNodes.Count;i++)
			{
				PathfindNode node = CompleteNodeArea.AllNodes[i];
				if(node.IsWalkableBy(walksTypes))
					AllowArea.AddNode(node.Point,node);					
				
			}
			return AllowArea;
//			Dictionary<Point,PathfindNode> nodes = new Dictionary<Point,PathfindNode>();
//			for(int i =0 ;i< CompleteNodeArea.AllNodes.Count;i++)
//			{
//				PathfindNode node = CompleteNodeArea.AllNodes[i];
//				if(node.IsWalkableBy(walksTypes))
//				   nodes.Add(node.Point,node);					
//				
//			}
//
//			return nodes;
		}

		public List<Point> GetWalkArea(int range,Point actorBasePosition,List<PathfindNode> unreachablePoints)
		{
			List<Point> TotalValidList = new List<Point>();
			List<AStarNode> ValidList = new List<AStarNode>();
			List<AStarNode> CheckedPoint = new List<AStarNode>();
			ValidList.Add(new AStarNode(new PathfindNode(actorBasePosition)));


			int loopMax = range*2;
			while(ValidList.Count > 0 && loopMax > 0)
			{
				loopMax--;
				List<AStarNode> LastValidList = new List<AStarNode>(ValidList);
				ValidList.Clear();
				foreach(AStarNode validPoint in LastValidList)
				{
					foreach(PathFindNeighbourg neighbourPath in Neighbourhood)
					{
						PathfindNode neighbour = new PathfindNode(neighbourPath.offset);
						PathfindNode pointToCheck = validPoint.Position+ neighbour;
						if(	unreachablePoints.Contains(pointToCheck)) continue;

						AStarNode nodeToCheck = new AStarNode(pointToCheck);
						nodeToCheck.fscore = validPoint.fscore + neighbourPath.cost;
						if(CheckedPoint.Contains(nodeToCheck)
						   || nodeToCheck.fscore > range) 
						{
							continue;
						}

						ValidList.Add(nodeToCheck);
						TotalValidList.Add(pointToCheck.Point);
						CheckedPoint.Add(nodeToCheck);
					}

				}
			}
			return TotalValidList; 

		}
		public PathfindNode GetNodeAt(Point p)
		{
			return CompleteNodeArea.GetNodeAt(p);
		
//			foreach(PathfindNode node in CompleteArea)
//			{
//				if(node.Point ==p)
//					return node;
//			}
//			return null;
		}

		public static void ResetPrecalculatedPath ()
		{
            PathFindingManager.PrecalculatedPaths = new Dictionary<PointCouple, Path>();
			instance.WaitingRunners.Clear();
			instance.Processing = false;
			//PathFindingManager.instance.PrecalcultateBridges();
		}
		public static void RegisterForRequest(FieldRunner runner)
		{
			if(instance.WaitingRunners.Contains(runner) == false)
				instance.WaitingRunners.Add(runner);
		}

		public static void NotifyPathReceived()
		{
			instance.Processing = false;
		}

//		public static IEnumerator AStartPathFinding(Point startPoint,Point endPoint,PathComplete Callback,Dictionary<Point,PathfindNode> walkableArea,WalkType walkType,int loopMaxCount = int.MaxValue )
//		{
//			PathfindNode start = instance.GetNodeAt(startPoint);			
//			PathfindNode end = instance.GetNodeAt(endPoint);
//
//			PathFindingRequestData requestData = new PathFindingRequestData();
//			requestData.StartNode = start;
//			requestData.EndNode = end;
//			requestData.Callback = Callback;
//			requestData.walkableArea = walkableArea;
//			requestData.walkType = walkType;
//			requestData.loopMaxCount = loopMaxCount;
//
//			instance.WaitingRequest.Add(requestData);
//			yield break;
//			//yield return instance.StartCoroutine( AStartPathFinding(start,end,Callback,walkableArea,walkType,loopMaxCount));			
//
//		}

		private bool Processing = false;
		private List<FieldRunner> WaitingRunners = new List<FieldRunner>();
		public void Update()
		{
			if(instance.Processing)
				return;

			if(WaitingRunners.Count >0)
			{
				//Debug.Log(WaitingRequest.Count);
				//Debug.Log("Processs : " + Time.realtimeSinceStartup);
				instance.Processing = true;
				PathFindingRequestData rqt = WaitingRunners[0].GetPathRequest();
				//Debug.Log("Last request: " + rqt.StartNode + " : " + rqt.EndNode + " : " + rqt.loopMaxCount);
                //StartCoroutine(PathFindingManager.AStartPathFinding(rqt.StartNode,rqt.EndNode,rqt.Callback,rqt.walkableArea,rqt.walkType,rqt.loopMaxCount));              
                WaitingRunners.RemoveAt(0);
				if(rqt != null)
					StartCoroutine(PathFindingManager.AStartPathFinding(rqt.StartNode,rqt.EndNode,rqt.Callback,rqt.walkableArea,rqt.walkType,rqt.loopMaxCount));
				else
					instance.Processing = false;	
			}
		}

		private static IEnumerator AStartPathFinding(PathfindNode startPoint,PathfindNode endPoint,PathComplete Callback,Dictionary<Point,PathfindNode> walkableArea,WalkType walkType,int loopMaxCount = int.MaxValue )
		{	

			if(startPoint.Point == endPoint.Point)
				Callback.Invoke(null);
			int startGroup = startPoint.GetParameter (walkType).Group;
			if(PathFindingManager.PrecalculatedPaths != null)
			{				
//                foreach(PointCouple couple in PathFindingManager.PrecalculatedPaths.Keys)
//                {
//                    //Debug.Log(couple);
//                    if(couple.StartPoint == startPoint.Point && couple.EndPoint == endPoint.Point)
//                    {
//                      //  Debug.Log("FIND COUPLE");
//                    }
//                }
                if(PathFindingManager.PrecalculatedPaths.ContainsKey(new PointCouple(startPoint.Point,endPoint.Point)))
				{		

					//instance.Processing = false;		
                    Callback.Invoke(PathFindingManager.PrecalculatedPaths[new PointCouple(startPoint.Point,endPoint.Point)].Clone());
					yield break;
				}
			}
		
			Dictionary<Point,PathfindNode> reachablePoints = walkableArea;// s_instance.CompleteArea;
			
			if(reachablePoints.ContainsKey(endPoint.Point) == false )
			{
				//Debug.Log("Node not reachable: "+endPoint.Point);
				//instance.Processing = false;	
				Callback.Invoke(null);
				yield break;
			}
//			if (BelongSameGroup (startPoint,endPoint,walkType) == false) {
//				
//				int endGroup = endPoint.GetParameter (walkType).Group;
//				List<int> walkedGroup = new List<int>();
//				List<PathfindNode> intermediates = new List<PathfindNode>();
//				intermediates.Insert(0,startPoint);
//				bool succes = GetIntermediatesPoint (startGroup, endGroup, walkType,ref walkedGroup,ref intermediates,endPoint);
//				if(succes == false)
//				{
//					//instance.Processing = false;	
//					Callback.Invoke(null);
//					yield break;
//				}
//				intermediates.Add(endPoint);
//				Enemy PathOwner = Callback.Target as Enemy;
//				PathOwner.ClearStepedPath();
//				for(int i =0;i<intermediates.Count-1;i++)
//				{
//					yield return instance.StartCoroutine (AStartPathFinding (intermediates [i], intermediates [i+1], PathOwner.BuildPathFromStepCallback, walkableArea, walkType, loopMaxCount));
//				}
//
//				//instance.Processing = false;	
//				Callback.Invoke(PathOwner.GetStepedPath());
//				yield break;
//			}
			List<AStarNode> openSet = new List<AStarNode>();
			Dictionary<Point,AStarNode> openSetDico = new Dictionary<Point,AStarNode>();
			openSet.Add(new AStarNode(startPoint));
			openSetDico.Add(startPoint.Point, openSet[openSet.Count-1]);
			Dictionary<Point,AStarNode> closedSet = new Dictionary<Point,AStarNode>();
			
			float fscore;// = (endPoint-startPoint).MagnitudeZero();
			
			fscore =Point.MagnitudeZero(endPoint.Point - startPoint.Point);//+(1 -Vector2.Dot(toNeighbor,toEnd))*20;
			
			int loopLimit = 0;
			while(openSet.Count > 0 && loopMaxCount > 0)
			{
				//Debug.Log(openSet.Count);
				loopMaxCount --;
				loopLimit ++;
				openSet.Sort();
				AStarNode current = openSet[0];
				if(current.Position == endPoint)
				{
					Path path = new Path(endPoint);
					while(current.Parent !=null)
					{
						path.steps.Add(current.Position);
						current = current.Parent;
					}
					path.steps.Reverse();
					
					
					
					
                    if(PathFindingManager.PrecalculatedPaths == null)
					{
                        PathFindingManager.PrecalculatedPaths = new Dictionary<PointCouple, Path>();
					}
					PointCouple couple = new PointCouple(startPoint.Point,endPoint.Point);
					if( PathFindingManager.PrecalculatedPaths.ContainsKey( couple))
					{
						Debug.Log("Try add existing couple: " + couple);
					}else
					{
						PathFindingManager.PrecalculatedPaths.Add(couple,path);
					}

					//instance.Processing = false;	
					Callback.Invoke(path.Clone());
					yield break;
				}
				openSetDico.Remove(openSet[0].Position.Point);
				openSet.RemoveAt(0);
				closedSet.Add(current.Position.Point, current);
				if(loopLimit > 10)
				{
					yield return new WaitForEndOfFrame();
					loopLimit = 0;
				}
				for (int i = 0; i < Neighbourhood.Length; i++) 
				{
					PathFindNeighbourg neighbourPath = Neighbourhood [i];
					PathfindNode neighbour = new PathfindNode (neighbourPath.offset);
					PathfindNode neigbourRealPosition = neighbour + current.Position;
					if (reachablePoints.ContainsKey (neigbourRealPosition.Point) == false) {
						continue;
					}
					AStarNode AstarNeighbor = new AStarNode (neigbourRealPosition);
					AstarNeighbor.Position = neigbourRealPosition;
					//current.Position;
					AstarNeighbor.Parent = current;
					if (closedSet.ContainsKey (AstarNeighbor.Position.Point)) {
						continue;
					}
					bool containsStartGroup = instance.CompleteNodeArea.GetNodeAt (neigbourRealPosition.Point).GetParameter (walkType).ContainsGroupOf (endPoint, walkType);
					if (containsStartGroup == false)
						continue;
					float tentativeGscore = current.gscore + neighbourPath.cost;
					AStarNode InSetNeighbor = null;
					//GetNodeInList(AstarNeighbor,openSet);
					if (openSetDico.ContainsKey (AstarNeighbor.Position.Point))
						InSetNeighbor = openSetDico [AstarNeighbor.Position.Point];
					if (InSetNeighbor == null || tentativeGscore < InSetNeighbor.gscore) {
						bool needAddToSet = false;
						if (InSetNeighbor == null) {
							InSetNeighbor = new AStarNode (neigbourRealPosition);
							needAddToSet = true;
						}
						InSetNeighbor.Parent = current;
						InSetNeighbor.gscore = tentativeGscore;
						InSetNeighbor.fscore = InSetNeighbor.gscore + heuristique (AstarNeighbor, endPoint.Point, current.Position.Point);
						if (needAddToSet) {
							AstarNeighbor.fscore = InSetNeighbor.fscore;
							AstarNeighbor.gscore = InSetNeighbor.gscore;
							openSet.Add (AstarNeighbor);
							openSetDico.Add (AstarNeighbor.Position.Point, openSet [openSet.Count - 1]);
						}
					}
				}
				
			}

			//instance.Processing = false;	
			Callback.Invoke(null);
			//return null;

		}

		static bool BelongSameGroup (PathfindNode startPoint,PathfindNode endPoint, WalkType walktype)
		{
			List<int> StartsGroups = new List<int>();
			foreach(int b in startPoint.GetParameter (walktype).GroupBridge)
			{
				StartsGroups.Add(b);
			}
			StartsGroups.Add(startPoint.GetParameter(walktype).Group);
			if(StartsGroups.Contains(endPoint.GetParameter(walktype).Group))
				return true;
		    foreach(int b in endPoint.GetParameter (walktype).GroupBridge)
		    {
				if(StartsGroups.Contains(b))
					return true;
			}
			return false;
			//return (startPoint.GetParameter(walkType).Group != endPoint.GetParameter (walkType).Group);
		}

		//return succes
		private static bool GetIntermediatesPoint(int startGroup,int endGroup,WalkType walkType,ref List<int> WalkedGroup,ref List<PathfindNode> intermediate,PathfindNode targetPoint)
		{
			List<PathfindNode> tmpIntermediate = new List<PathfindNode>();
			List<PathfindNode> stepIntermediate= new List<PathfindNode>();
			for (int bri = instance.Bridges.Count-1; bri >=0; bri--) 
			{
				PathfindNode bridge = instance.Bridges [bri];
				PathfindNodeGroupParameter groupParameter = bridge.GetParameter (walkType);
				if (groupParameter.Group == startGroup || groupParameter.Group == endGroup) 
				{
					for (int i = 0; i < groupParameter.GroupBridge.Count; i++) 
					{
						int bridgeGroup = groupParameter.GroupBridge [i];
						if (bridgeGroup == endGroup || bridgeGroup == startGroup) {
							groupParameter.GroupBridge [i] = endGroup;
							groupParameter.Group = startGroup;
							tmpIntermediate.Add (bridge);
						}
					}
				}
			}
			if(tmpIntermediate.Count > 0)
			{
				int bestBridge =GetBestBridgeIn (walkType, tmpIntermediate,intermediate [intermediate.Count - 1],targetPoint);				
				intermediate.Add (tmpIntermediate [bestBridge]);
				PathfindNodeGroupParameter NextPointIntermediate = tmpIntermediate [bestBridge].GetParameter (walkType);
				return true;
			}else
			{
				WalkedGroup.Add(startGroup);
				for (int bri = instance.Bridges.Count-1; bri >=0; bri--) 
				{
					PathfindNode bridge = instance.Bridges [bri];
					PathfindNodeGroupParameter groupParameter = bridge.GetParameter (walkType);
					List<int> bridgeGroups = new List<int>(groupParameter.GroupBridge);
					bridgeGroups.Add(groupParameter.Group);
					if (bridgeGroups.Contains(startGroup) ) 
					{
						for(int i = 0;i< bridgeGroups.Count;i++)
						{	
							if(WalkedGroup.Contains( bridgeGroups[i]) == false)
							{
								int newGroupIndex =  bridgeGroups[i];
								List<PathfindNode> localBridges = GetAllBridges(bridgeGroups[i],startGroup,walkType);
								int bestBridgeIndex =GetBestBridgeIn(walkType,localBridges,intermediate [intermediate.Count - 1],targetPoint);
								PathfindNode bestBridge = null;
								if(bestBridgeIndex >=0)
									bestBridge = localBridges[bestBridgeIndex];
								else
								{
									Debug.Log("can't find bestbrige");
									Debug.Break();
								}
								intermediate.Add(bestBridge);
								if(GetIntermediatesPoint(bridgeGroups[i],endGroup,walkType,ref WalkedGroup,ref intermediate,targetPoint))
								{
									return true;
								}else
								{									
									intermediate.Remove(bestBridge);
								}
							}
						}
					}
				}
				return false;
			}



		}

		static List<PathfindNode> GetAllBridges (int from, int to,WalkType walkType)
		{
			List<PathfindNode> tmpIntermediate = new List<PathfindNode>();
			for (int bri = instance.Bridges.Count-1; bri >=0; bri--) 
			{
				PathfindNode bridge = instance.Bridges [bri];
				PathfindNodeGroupParameter groupParameter = bridge.GetParameter (walkType);
				if (groupParameter.Group == from || groupParameter.Group == to) 
				{
					for (int i = 0; i < groupParameter.GroupBridge.Count; i++) 
					{
						int bridgeGroup = groupParameter.GroupBridge [i];
						if (bridgeGroup == to || bridgeGroup == from) {
							tmpIntermediate.Add (bridge);
						}
					}
				}
			}
			return tmpIntermediate;
		}

		static int GetBestBridgeIn (WalkType walkType, List<PathfindNode> tmpIntermediate,PathfindNode previousPoint,PathfindNode endPoint)
		{
			int bestBridge = -1;
			float bestDistance = float.MaxValue;
			for (int i = 0; i < tmpIntermediate.Count; i++) 
			{
				//float dist = (tmpIntermediate [i].Point - previousPoint.Point).EuclidiantMagnitude ();
				float dist = (tmpIntermediate[i].Point.ToVector2() - previousPoint.Point.ToVector2()).magnitude;//NawakTools.GetDistanceToLine(tmpIntermediate [i].Point.ToVector2(), previousPoint.Point.ToVector2(),endPoint.Point.ToVector2());//(tmpIntermediate [i].Point - previousPoint.Point).EuclidiantMagnitude ();
				float weight = 0.3f;
				//Debug.Log("befor: " + dist);
				dist += weight*(previousPoint.Point - tmpIntermediate [i].Point).Magnitude();
				//dist += weight*(endPoint.Point - tmpIntermediate [i].Point).Magnitude();
				//Debug.Log("after: " + dist);
				if (dist < bestDistance) {
					bestBridge = i;
					bestDistance = dist;
				}
			}
			if(bestBridge == -1)
			{
				Debug.Break();
			}
			//Debug.Log("bestbridgr :" + tmpIntermediate[bestBridge] + " : " + previousPoint + " : " + endPoint);
			return bestBridge;
		}

		public static Path AStartPathFinding(PathfindNode startPoint,PathfindNode endPoint,Dictionary<Point,PathfindNode> reachablePoints,int loopMaxCount = int.MaxValue)
		{
			if(reachablePoints.ContainsKey(endPoint.Point) == false) return null;
			
            if(PathFindingManager.PrecalculatedPaths != null && PathFindingManager.PrecalculatedPaths.ContainsKey(new PointCouple(startPoint.Point,endPoint.Point)))
			{				
                return PathFindingManager.PrecalculatedPaths[new PointCouple(startPoint.Point,endPoint.Point)].Clone();
			}


			List<AStarNode> openSet = new List<AStarNode>();
			Dictionary<Point,AStarNode> openSetDico = new Dictionary<Point,AStarNode>();
			openSet.Add(new AStarNode(startPoint));
			openSetDico.Add(startPoint.Point, openSet[openSet.Count-1]);
			Dictionary<Point,AStarNode> closedSet = new Dictionary<Point,AStarNode>();

			float fscore;// = (endPoint-startPoint).MagnitudeZero();
			
				fscore =Point.MagnitudeZero(endPoint.Point - startPoint.Point);//+(1 -Vector2.Dot(toNeighbor,toEnd))*20;
			
			int loopLimit = loopMaxCount;
			while(openSet.Count > 0 && loopLimit > 0)
			{
				//Debug.Log(openSet.Count);
				loopLimit --;
				openSet.Sort();
				AStarNode current = openSet[0];
				if(current.Position == endPoint)
				{
					Path path = new Path(endPoint);
					while(current.Parent !=null)
					{
						path.steps.Add(instance.GetNodeAt( current.Position.Point));
						current = current.Parent;
					}
					path.steps.Reverse();
                    if(PathFindingManager.PrecalculatedPaths == null)
					{
                        PathFindingManager.PrecalculatedPaths = new Dictionary<PointCouple, Path>();
					}
                    PathFindingManager.PrecalculatedPaths.Add(new PointCouple(startPoint.Point,endPoint.Point),path);
					return path.Clone();
				}
				openSetDico.Remove(openSet[0].Position.Point);
				openSet.RemoveAt(0);
				if(closedSet.ContainsKey(current.Position.Point))
				{
					int fdfg = 4;
				}
				closedSet.Add(current.Position.Point, current);
				for (int i = 0, NeighbourhoodLength = Neighbourhood.Length; i < NeighbourhoodLength; i++) 
				{
					PathFindNeighbourg neighbourPath = Neighbourhood [i];
					PathfindNode neighbour = new PathfindNode (neighbourPath.offset);
					PathfindNode neigbourRealPosition = neighbour + current.Position;
					if (reachablePoints.ContainsKey (neigbourRealPosition.Point) == false) {
						continue;
					}
					AStarNode AstarNeighbor = new AStarNode (neigbourRealPosition);
					AstarNeighbor.Position = neigbourRealPosition;
					//current.Position;
					AstarNeighbor.Parent = current;
					if (closedSet.ContainsKey (AstarNeighbor.Position.Point)) {
						continue;
					}
					float tentativeGscore = current.gscore + neighbourPath.cost;
					AStarNode InSetNeighbor = null;
					//GetNodeInList(AstarNeighbor,openSet);
					if (openSetDico.ContainsKey (AstarNeighbor.Position.Point))
						InSetNeighbor = openSetDico [AstarNeighbor.Position.Point];
					if (InSetNeighbor == null || tentativeGscore < InSetNeighbor.gscore) {
						bool needAddToSet = false;
						if (InSetNeighbor == null) {
							InSetNeighbor = new AStarNode (neigbourRealPosition);
							needAddToSet = true;
						}
						InSetNeighbor.Parent = current;
						InSetNeighbor.gscore = tentativeGscore;
						InSetNeighbor.fscore = InSetNeighbor.gscore + heuristique (AstarNeighbor, endPoint.Point, current.Position.Point);
						if (needAddToSet) {
							AstarNeighbor.fscore = InSetNeighbor.fscore;
							AstarNeighbor.gscore = InSetNeighbor.gscore;
							openSet.Add (AstarNeighbor);
							openSetDico.Add (AstarNeighbor.Position.Point, openSet [openSet.Count - 1]);
						}
					}
				}

			}
			return null;
		}

		static AStarNode GetNodeInList (AStarNode astarNeighbor,List<AStarNode> openset)
		{
			for (int i = 0, opensetCount = openset.Count; i < opensetCount; i++) 
			{
				AStarNode node = openset [i];
				if (node == astarNeighbor)
					return node;
			}
			return null;
		}

		static float heuristique (AStarNode astarNeighbor, Point endPoint,Point current)
		{
			Vector2 toNeighbor = (astarNeighbor.Position.Point - current).ToVector2().normalized;
			Vector2 toEnd = ( endPoint - current).ToVector2().normalized;
			//return (1 -Vector2.Dot(toNeighbor,toEnd)) *10 ;
			             //+ (endPoint - astarNeighbor.Position).ToVector2().magnitude;
			//float dist = (endPoint - astarNeighbor.Position).ToVector2().magnitude; 
			float dist =0;
				dist =Point.MagnitudeZero(endPoint - astarNeighbor.Position.Point);//+(1 -Vector2.Dot(toNeighbor,toEnd))*20;
			
			return dist;
		}
		
		private static Dictionary<PointCouple,Path> PrecalculatedPaths;
	}
	public class PointCouple:IComparable<PointCouple>
	{
		public Point StartPoint;
		public Point EndPoint;
		
		public PointCouple(Point start,Point end)
		{
			StartPoint = start;
			EndPoint = end;
		}

		public override string ToString ()
		{
			return StartPoint.ToString() +" : "+EndPoint.ToString();
		}

		#region IComparable implementation
		int IComparable<PointCouple>.CompareTo (PointCouple other)
		{
			if(StartPoint == other.StartPoint && EndPoint == other.EndPoint)
			{
				return 0;
			}else
			{
				if(StartPoint == other.StartPoint)
					return 1;
				return -1;
					
			}
		}
		public override int GetHashCode()
		{
			return StartPoint.GetHashCode() +EndPoint.GetHashCode();
		}
		public override bool Equals (object otherObj)
		{
			PointCouple other = otherObj as PointCouple;
			return StartPoint == other.StartPoint && EndPoint == other.EndPoint;
		}
		
		#endregion
	}
	public class AStarNode:IComparable<AStarNode>,IEquatable<AStarNode>
	{
		public AStarNode Parent;
		public float fscore;
		public float gscore;
		public PathfindNode Position;

		public AStarNode(PathfindNode p)
		{
			fscore = 0;
			gscore = 0;
			Position = p;
		}

		#region IComparable implementation

		public int CompareTo (AStarNode other)
		{
			if( fscore <other.fscore)
				return-1;
			else return 1;
			//return (int)(fscore - other.fscore);
		}

		#endregion

		#region IEquatable implementation

		public bool Equals (AStarNode other)
		{
			if ((object)other == null)
			{
				return false;
			}    
			return Position == other.Position;
		}
		public static bool Equals(AStarNode p1,AStarNode p2)
		{
			if (object.ReferenceEquals( p1, p2)) {
				// handles if both are null as well as object identity
				return true;
			}
			if ((object)p1 == null || (object)p1 == null)
			{
				return false;
			}  
			return p1.Equals(p2);
		}
		
		public static bool operator ==(AStarNode p,AStarNode p2)
		{	
			if (object.ReferenceEquals( p, p2)) {
				return true;
			}
			return p.Equals(p2);			
		}
		public static bool operator !=(AStarNode p,AStarNode p2)
		{ 
			if (object.ReferenceEquals( p, p2)) {
				return false;
			}
			return p.Equals(p2) == false;
		}

		#endregion
	}
	[Serializable]
	public class Path
	{
		public PathfindNode Target;
		public List<PathfindNode> steps;
		public float length
		{
			get{return steps.Count;}
		}

		public Path()
		{			
			steps = new List<PathfindNode>();
		}

		public PathFinding.Path Clone ()
		{
			Path path = new Path();
			path.steps = new List<PathfindNode>(steps);
			
			return path;
		}

		public Path(PathfindNode p ):this()
		{
			Target = p;
		}

		public Path (string str)
		{
			steps = new List<PathfindNode>();
			string[] nodesStr = str.Split('[',']');
			foreach(string node in nodesStr)
			{
				if(string.IsNullOrEmpty(node))continue;
				string[] coords = node.Split(':');
				Point p = new Point();
				int.TryParse(coords[0],out p.X);
				int.TryParse(coords[1],out p.Y);

				steps.Add(PathFindingManager.instance.GetNodeAt(p));
			}
		}

		public int ContainsStepIndex (Point p,bool returnNearestPoint = false,int maxIndex = int.MaxValue)
		{
			int index = 0;
			int nearestIndex = -1;
			float nearestDist = float.MaxValue;
			
			maxIndex = Mathf.Min(maxIndex,steps.Count-1);
			
			//for (int i = 0; i < steps.Count; i++) 
			for (int i = steps.Count-1; i >=0; i--) 
			{
				PathfindNode step = steps [i];
				if (step.Point == p) {
					return index;
				}
				if (returnNearestPoint && (step.Point - p).Magnitude () < nearestDist) {
					nearestDist = (step.Point - p).Magnitude ();
					nearestIndex = i;
				}
				index = i;
			}
			if(returnNearestPoint)
			{
				//Debug.Log(nearestDist);
				return nearestIndex;
			}
			return -1;
		}

		public bool ContainsStep (Point p)
		{
			return ContainsStepIndex(p)>=0;
		}

		public void MoveStep (int index, Point position)
		{
			if(ContainsStep(position))
			   steps.RemoveAt(index);
		   else
				steps[index].Point = position;
		}
		public override string ToString ()
		{
			string s = "";
			foreach(PathfindNode node in steps)
			{
				s += "["+node.Point.ToString()+"]";
			} 
			return s;
		}
	}

	public class PathFindingRequestData
	{
		public PathfindNode StartNode;
		public PathfindNode EndNode;
		public PathFinding.PathFindingManager.PathComplete Callback;
		public Dictionary<Point,PathfindNode> walkableArea;
		public WalkType walkType;
		public int loopMaxCount;



	}

}
