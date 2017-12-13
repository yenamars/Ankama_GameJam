using UnityEngine;
using System.Collections;
using System;
using PathFinding;
using System.Collections.Generic;
using System.Linq;

public class NawakTools : MonoBehaviour {

	
	public static Mesh BuildMesh(float tileSize,int pointCount = 6)
	{
		if( pointCount == 4)
			return BuildSquare(tileSize);
		return BuildHexagone(tileSize);
	}
	private static Mesh BuildSquare(float tileSize)
	{
		Mesh m_mesh; 
		int pointCount = 4;
		Vector3[] points = new Vector3[pointCount+1];
		Vector2[] uv = new Vector2[pointCount+1];
		Vector3[] normal = new Vector3[pointCount+1];
		Color[] color = new Color[pointCount+1];
		points[0] = Vector3.zero;
		uv[0] = Vector2.one/2.0f;
		normal[0] = Vector3.zero;
		color[0] = Color.white;
		
		m_mesh = new Mesh();	
		for(int i = 0;i<pointCount;i++)
		{
			Vector3 rotVector = Rotate(Vector3.right,i*(360.0f/pointCount)*Mathf.PI/180+(180.0f/pointCount)*Mathf.PI/180);
			uv[i+1] = new Vector2((rotVector.x+1)/2.0f,(rotVector.z+1)/2.0f);
			points[i+1] = tileSize*rotVector*Mathf.Sqrt(2.0f) ;
			normal[i+1] = Vector3.zero;
			color[i+1] = Color.white;
			
		}
		m_mesh.vertices = points;
		m_mesh.uv = uv;
		m_mesh.colors = color;
		m_mesh.normals = normal;
		m_mesh.triangles =getTriangleFromPointCount(pointCount);// new int[6*3]{0,1,2,0,2,3,0,3,4,0,4,5,0,5,6,0,6,1};
		m_mesh.name = "superHexagone";
		return m_mesh;
	}
	private static Mesh BuildHexagone(float tileSize)
	{
		Mesh m_mesh; 
		int pointCount = 6;
		Vector3[] points = new Vector3[pointCount+1];
		Vector2[] uv = new Vector2[pointCount+1];
		Vector3[] normal = new Vector3[pointCount+1];
		Color[] color = new Color[pointCount+1];
		points[0] = Vector3.zero;
		uv[0] = Vector2.one/2.0f;
		normal[0] = Vector3.zero;
		color[0] = Color.white;
		
		m_mesh = new Mesh();	
		for(int i = 0;i<pointCount;i++)
		{
			Vector3 rotVector = Rotate(Vector3.right,i*(360.0f/pointCount)*Mathf.PI/180+(180.0f/pointCount)*Mathf.PI/180);
			uv[i+1] = new Vector2((rotVector.x+1)/2.0f,(rotVector.z+1)/2.0f);
			points[i+1] = tileSize*rotVector ;
			normal[i+1] = Vector3.zero;
			color[i+1] = Color.white;
			
		}
		m_mesh.vertices = points;
		m_mesh.uv = uv;
		m_mesh.colors = color;
		m_mesh.normals = normal;
		m_mesh.triangles =getTriangleFromPointCount(pointCount);// new int[6*3]{0,1,2,0,2,3,0,3,4,0,4,5,0,5,6,0,6,1};
		m_mesh.name = "superHexagone";
		return m_mesh;
	}

	private static int[] getTriangleFromPointCount(int pointCount)
	{
		if(pointCount ==6)
		{
			return new int[6*3]{0,1,2,0,2,3,0,3,4,0,4,5,0,5,6,0,6,1};;
		}
		
		if(pointCount ==4)
		{
			return new int[4*3]{0,1,2,0,2,3,0,3,4,0,4,1};;
		}
		return null;
	}

	public static Vector3 ScreenToWorld(Vector3 screenPos)
	{
		//Debug.Log(screenPos);
		Plane p = new Plane(Vector3.up,Vector3.zero);
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		float enterOffset;
		if(p.Raycast(ray,out enterOffset))
		{
			return ray.origin+enterOffset*ray.direction;
		}
		return Vector3.zero;
	}
	public static Vector3 WorldToScreen(Vector3 worldPosition)
	{

		return Camera.main.WorldToScreenPoint(worldPosition);

	}
	public static Vector3 Rotate(Vector3 v,float a)
	{
		float cos =(float) Mathf.Cos(a);
		float sin = (float)Mathf.Sin(a);
		return new Vector3(v.x*cos+v.z*sin,v.y,v.z*cos-v.x*sin);
	}
	public static int s_tileSize;
	public static  Vector3 cellToWorld(Point cellPosition)
	{
		return cellToWorld(cellPosition.ToVector2());
	}
	public static Vector3 cellToWorld(Vector2 cellPosition)//,TileFormat format)
	{
			return new Vector3(
				cellPosition.x,
				0,
				cellPosition.y)*s_tileSize*2;//1*Mathf.Sqrt(2);
		
		
	}
	public static Point worldPositionToCellCenter (Vector3 worldPosition)
	{
		Vector3 localWorldPosition = worldPosition;
		
		localWorldPosition = NawakTools.Rotate(localWorldPosition,0);//-transform.eulerAngles.y*Mathf.PI/180);

		Vector2 cellCenter = Vector2.zero;
			cellCenter = new Vector2((int)(( localWorldPosition.x +s_tileSize)/(2*s_tileSize))
			                         ,(int)( (localWorldPosition.z+s_tileSize)/(2*s_tileSize)));
			//Debug.Log(localWorldPosition+" : "+cellCenter);
			
		
		return new Point(cellCenter);
	}	

	public static float GetDistanceToLine(Vector2 pointPosition,Vector2 startPoint,Vector2  endPoint)
	{
		Vector3 direction = endPoint - startPoint;
		//direction.y = 0;
		if(direction.x ==0)
			return Mathf.Abs( pointPosition.x - endPoint.x) + Mathf.Abs( pointPosition.y - endPoint.y);
		
		float m = (direction.y )/(direction.x);
		float b =endPoint.y - m * endPoint.x;
		float numerator = Mathf.Abs(m* pointPosition.x - pointPosition.y + b);
		float denominator = Mathf.Sqrt(m*m+1);
		
		
		float distanceToLine = numerator/denominator;
		direction.Normalize();
		float distance =(startPoint - endPoint).magnitude + 0.5f;
		float distanceToPoint = (pointPosition -endPoint).magnitude;
		float dot = Vector2.Dot(pointPosition-endPoint,pointPosition-startPoint);
		if( distanceToPoint > distance && dot >0)
		{
			distanceToLine = Mathf.Min( (pointPosition -endPoint).magnitude,(pointPosition -startPoint).magnitude);
		}
		return distanceToLine;
	}
	public static float DistanceFromCenter (Point p, Vector3 worldPosition)
	{
		Vector3 localWorldPosition = worldPosition;
		
		localWorldPosition = NawakTools.Rotate(localWorldPosition,0);//-transform.eulerAngles.y*Mathf.PI/180);
		
		Vector2 cellCenter = Vector2.zero;
			
			cellCenter = new Vector2((int)(( localWorldPosition.x +s_tileSize)/(2*s_tileSize))
			                         ,(int)( (localWorldPosition.z+s_tileSize)/(2*s_tileSize)));
			Vector2 tileposition = new Vector2((( localWorldPosition.x +s_tileSize)/(2*s_tileSize))
			                         ,( (localWorldPosition.z+s_tileSize)/(2*s_tileSize)));
			return (tileposition - cellCenter - Vector2.one*0.5f).magnitude*Mathf.Sqrt(2);
		
	}
	
	public static Path OptimisePath(Path path)
	{
		Path newPath = path.Clone();
		for(int i = 1;i<newPath.steps.Count -1 ; i++)
		{
			Point curentPoint = newPath.steps[i].Point;
			
			Point nextPoint = newPath.steps[i+1].Point;
			Point previousPoint = newPath.steps[i-1].Point;
			
			float totalMagnitude = Point.Magnitude(nextPoint - curentPoint) + Point.Magnitude (curentPoint - previousPoint);
			float shortcutMagnitude = Point.Magnitude(nextPoint - previousPoint) ;
			
			if(shortcutMagnitude < totalMagnitude && shortcutMagnitude < 2 && shortcutMagnitude> 0)
			{
				newPath.steps.RemoveAt(i);
				//i--;
			}
		}
		return newPath;
	}
	
	public static void OptimisePath (ref Path path)
	{
		path = OptimisePath(path);
		
	}
	

	public static string GetLangExtension ()
	{
		if(Application.systemLanguage == SystemLanguage.French)
			return "FR";
		return "FR";
		return "EN";
	}
}