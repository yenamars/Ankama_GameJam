using UnityEngine;
using System.Collections;
using PathFinding;

public interface IPathFollower {

	void BuildPathFromStepCallback (Path path);
	
	void ClearStepedPath ();
	Path GetStepedPath();
}
