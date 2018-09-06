using System.Collections.Generic;
using UnityEngine;

public class SimpleVisionSensorComponent : VisionSensorComponentBase
{
	public HashSet<int> knownIDs = new HashSet<int>();

	public override void Lose(GameObject other)
	{
		knownIDs.Remove(other.GetInstanceID());
	}

	public override void Perceive(GameObject other)
	{
		knownIDs.Add(other.GetInstanceID());
	}

}