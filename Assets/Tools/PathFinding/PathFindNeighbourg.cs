using UnityEngine;
using System.Collections;

namespace PathFinding
{
	public class PathFindNeighbourg  {

		public Point offset;
		public float cost;
		public PathFindNeighbourg(Point p, float cout = 1)
		{
			offset = p;
			cost = cout;
		}
	}
}
