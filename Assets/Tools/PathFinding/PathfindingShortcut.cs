using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathFinding;

public class PathfindingShortcut 
{
	public List<PathfindNode> InitialWay;
	public List<PathfindNode> Shortcut;

	public PathfindNode StartNode
	{
		get{ return Shortcut[0];}
	}
}
