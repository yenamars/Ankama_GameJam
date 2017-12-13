using System;
using UnityEngine;

[Serializable]
public class Point:IEquatable<Point>
{
	public static Point Zero 
	{
		get{return new Point(0,0);}
	} 

	public int X;
	public int Y;
	public Point()
	{}

	public Point( int x,int y)
	{
		X =x;		
		Y =y;
	}

	public Point( Vector2 pos)
	{
		X = (int)pos.x;		
		Y = (int)pos.y;
	}
	public Point( Point p)
	{
		X = p.X;		
		Y = p.Y;
	}
	public Vector2 ToVector2()
	{
		return new Vector2(X,Y);
	}

	public static Point operator +(Point p,Point p2)
	{
		return new Point (p.X + p2.X, p.Y + p2.Y);
	}
	public static Point operator *(Point p,int i)
	{
		return i * p;
	}
	public static Point operator *(int i,Point p)
	{
		return new Point (p.X *i, p.Y *i);
	}

	public static bool Equals(Point p1,Point p2)
	{
		return p1.X == p2.X && p1.Y == p2.Y;
	}

	public static Point operator -(Point p,Point p2)
	{
		return new Point (p.X - p2.X, p.Y - p2.Y);
	}
	public override int GetHashCode()
	{
		return X.GetHashCode() ^ Y.GetHashCode();
	}
	public static bool operator ==(Point p,Point p2)
	{
		return p.Equals(p2);
	}
	public static bool operator !=(Point p,Point p2)
	{
		return p.Equals(p2) == false;
	}

	public Point Normalized()
	{
		return new Point(X/MagnitudeZero(),Y/MagnitudeZero());
	}
	
	public int MagnitudeZero()
	{
		return Point.MagnitudeZero (this);
	}
	public static int MagnitudeZero(Point p)
	{
		return Math.Abs (p.Y) + Math.Abs (p.X);
	}
	public  float EuclidiantMagnitude ()
	{
		return Point.EuclidiantMagnitude(this);
	}
	public static float EuclidiantMagnitude (Point p)
	{
		int XAbs = Mathf.Abs(p.X);
		int YAbs = Mathf.Abs(p.Y);
		return Math.Min(XAbs,YAbs)*Mathf.Sqrt(2) + Math.Abs(XAbs - YAbs);
	}

	public float Magnitude()
	{
		return Point.Magnitude (this);
	}
	public static float Magnitude(Point p)
	{
		return (float)Math.Sqrt(p.X*p.X + p.Y*p.Y);
	}
	public static float HexaNorm(Point p)
	{
		return p.HexaNorm();
	}
	public float HexaNorm()
	{		
		return Mathf.Max(Mathf.Abs(X),Mathf.Abs(Y),Mathf.Abs(-X-Y));
	}

	public override string ToString ()
	{
		return X+" : "+Y;
	}
	#region IEquatable implementation

	public bool Equals (Point other)
	{
		return X == other.X && Y == other.Y;
	}

	#endregion
}