using System.Collections;
using System.Collections.Generic;
using PathFinding;
using UnityEngine;

public class FieldRunner : MonoBehaviour {

	public virtual PathFindingRequestData GetPathRequest()
	{
		return null;
	}
}
