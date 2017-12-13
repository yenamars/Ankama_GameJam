using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathFinding;
using System;

[Serializable]
public class NodesArea {

	
	public int Width;
	public int Height;
	public List<PathfindNode> AllNodes;


	public PathfindNode[,] ArrayArea;
	public Dictionary<Point,PathfindNode> DicoArea;

	public void BuildArea()
	{
		ArrayArea = new PathfindNode[Width+1,Height+1];
		DicoArea = new Dictionary<Point, PathfindNode>();
		foreach(PathfindNode node in AllNodes)
		{
			ArrayArea[node.Point.X,node.Point.Y] = node;
			DicoArea.Add(node.Point,node);
		}
	}
	public void OnAfterDeserialize()
	{
		BuildArea();
	}
	public NodesArea(int width,int height)
	{
		Width = width;
		Height = height;
		Clear();
	}

	public bool Contains (PathfindNode curentPoint)
	{
		return GetNodeAt(curentPoint.Point) != null;
		return AllNodes.Contains(curentPoint);
	}
	
	public void AddNode(Point p)
	{
		PathfindNode newNode = new PathfindNode(p);
		AddNode(p,newNode);
		
	}
	public void AddNode(Point p,PathfindNode newNode)
	{
		AllNodes.Add(newNode);
		ArrayArea[p.X,p.Y] = newNode;
		DicoArea.Add(p,newNode);
		
	}

	public PathfindNode GetNodeAt (Point p)
	{
		
		if(ArrayArea == null)
			BuildArea();
		
		return ArrayArea[Mathf.Clamp(p.X,0,ArrayArea.GetLength(0)-1),Mathf.Clamp(p.Y,0,ArrayArea.GetLength(1)-1)];
		if(p.X>=0 && p.Y>=0&& p.X < ArrayArea.GetLength(0) && p.Y < ArrayArea.GetLength(1))
			return ArrayArea[p.X,p.Y];
		Debug.LogError("Node NoteFound");
//		foreach(PathfindNode node in AllNodes)
//		{
//			if(node.Point ==p)
//				return node;
//		}
		return null;
	}
	public bool IsNodeInside(Point p)
	{
		return p.X>=0 && p.Y>=0&& p.X < ArrayArea.GetLength(0) && p.Y < ArrayArea.GetLength(1);
	}
	public void Clear ()
	{
		ArrayArea = new PathfindNode[Width+1,Height+1];
		AllNodes = new List<PathfindNode>();
		DicoArea = new Dictionary<Point, PathfindNode>();
	}

}
