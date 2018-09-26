using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisionObjectProvider
{
	IEnumerable<GameObject> GetObjectsWithinRange(Vector3 position, float radius);
}