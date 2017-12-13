using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace PathFinding
{
[Serializable]
	public class PathfindNode: IComparable<PathfindNode>,IEquatable<PathfindNode>
	{
		public Point Point;
		public float Weight;

		public List<PathfindNodeGroupParameter> listParameters;
		//private Dictionary<WalkType,PathfindNodeGroupParameter> Parameters;

		public bool Selected;
		public PathfindNode()
		{}
		public PathfindNode (Point point)
		{
			CreateParameter ();
			Point = point;
			Weight = 1;
		}
		public PathfindNode (PathfindNode node):this(node.Point)
		{
			Weight = node.Weight;

			listParameters = new List<PathfindNodeGroupParameter>(node.listParameters);
			//Parameters = new Dictionary<WalkType, PathfindNodeGroupParameter>(node.Parameters);
		}
		public PathfindNode (Point point,bool selected):this(point)
		{
			Selected = selected;
		}

		void CreateParameter ()
		{
			
			listParameters =new List<PathfindNodeGroupParameter>();
			listParameters.Add ( new PathfindNodeGroupParameter (WalkType.Walk));
			listParameters.Add ( new PathfindNodeGroupParameter (WalkType.Fly));

//			Parameters = new Dictionary<WalkType, PathfindNodeGroupParameter> ();
//			Parameters.Add (WalkType.Walk, new PathfindNodeGroupParameter (WalkType.Walk));
//			Parameters.Add (WalkType.Fly, new PathfindNodeGroupParameter (WalkType.Fly));
		}
//		public void BuildParameter()
//		{			
//			CreateParameter();
//			if(Walkable == false)
//			{
//				int t=0;
//				t++;
//			}
//			GetParameter((int)WalkType.Walk).Enable = Walkable;
//			GetParameter((int)WalkType.Fly).Enable = Flyable;
//			
//			GetParameter((int)WalkType.Walk).Group = Group;
//			GetParameter((int)WalkType.Fly).Group = 0;
//		}
		public PathfindNodeGroupParameter GetParameter (WalkType type)
		{
			if(listParameters == null)
				CreateParameter();
			
			foreach(PathfindNodeGroupParameter param in listParameters)
			{
				if(param.WalkType == type)
					return param;
			}
			PathfindNodeGroupParameter newparam = new PathfindNodeGroupParameter(type);
			listParameters.Add(newparam);
			
			return newparam;
		}
		public PathfindNodeGroupParameter GetParameter (int m_curentTab)
		{
			WalkType type = (WalkType)m_curentTab;
			return GetParameter(type);

		}
		
		public bool IsWalkableBy (List<WalkType> walksTypes)
		{
			foreach(WalkType type in walksTypes)
			{
				if(GetParameter(type).Enable == true)
					return true;
			}
			
			return false;
		}
		
		#region IComparable implementation
		
		public int CompareTo (PathfindNode other)
		{
			if( Weight <other.Weight)
				return-1;
			else return 1;
		}
		
		#endregion
		
		#region IEquatable implementation
		
		public bool Equals (PathfindNode other)
		{
			if ((object)other == null)
			{
				return false;
			}    
			return Point == other.Point;
		}
		public static bool Equals(PathfindNode p1,PathfindNode p2)
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
		
		public static bool operator ==(PathfindNode p,PathfindNode p2)
		{	
			if (object.ReferenceEquals( p, p2)) {
				
				return true;
			}
			return p.Equals(p2);			
		}
		public static bool operator !=(PathfindNode p,PathfindNode p2)
		{ 
			if (object.ReferenceEquals( p, p2)) {
				return false;
			}
			return p.Equals(p2) == false;
		}
		
		#endregion
		
		public static PathfindNode operator +(PathfindNode p,PathfindNode p2)
		{
			PathfindNode node = new PathfindNode(p.Point+p2.Point);
			node.Weight = Mathf.Min(p.Weight,p2.Weight);
//			node.Walkable = p.Walkable && p2.Walkable;
//			node.Flyable = p.Flyable && p2.Flyable;
			return node;
		}
		public override string ToString ()
		{
			return Point.ToString() + " ,walkable: "+GetParameter(WalkType.Walk).Enable+ " ,flyable: "+GetParameter(WalkType.Fly).Enable;
		}
	}

	[Serializable]
	public class PathfindNodeGroupParameter
	{
		public WalkType WalkType;
		public bool Enable;
		public int Group;

		public List<int> GroupBridge;

		public bool ContainsGroup(int group)
		{
			return Group == group || GroupBridge.Contains(group);
		}

		public bool ContainsGroupOf(PathfindNode node,WalkType walks)
		{
//			if(node.Point == new Point(45,23))
//			{
//				int dsd=0;
//				dsd +=1;
//			}		
			if(ContainsGroup(node.GetParameter(WalkType).Group ))
				return true;
			foreach(int group in node.GetParameter(WalkType).GroupBridge)
				if( ContainsGroup(group))
					return true;

			return false;
		}

		public PathfindNodeGroupParameter()
		{
			Enable = true;
			Group = 0;
		}
		public PathfindNodeGroupParameter(WalkType type):this()
		{
			WalkType = type;
		}
	}

}

public enum WalkType
{
	Walk = 0,
	Fly,
}