using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSensorComponent : MonoBehaviour
{
	float refreshRate = 1f;
	float range = 10f;
	public IVisionObjectProvider objectProvider;

	public HashSet<int> knownIDs = new HashSet<int>();

	void Awake()
	{
		if (objectProvider == null) objectProvider = InterfaceHelper.FindObject<IVisionObjectProvider>();
	}

	void OnEnable()
	{
		StartCoroutine(Recheck());
	}

	IEnumerator Recheck()
	{
		while (true)
		{
			var line = transform.forward * range;
			for (float angle = 1; angle <= 360; angle += 45)
			{
				var rotatedLine = Quaternion.AngleAxis(angle, transform.up) * line;
				Debug.DrawLine(transform.position, transform.position + rotatedLine, Color.blue, 1f);
			}
			foreach (var obj in objectProvider.GetObjectsWithinRange(transform.position, range))
			{
				if (obj != this.gameObject && IsVisible(obj))
					Perceive(obj);
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}

	protected virtual bool IsVisible(GameObject other)
	{
		Debug.Log("VisionSensoryCompoment:IsVisible");
		return true;
	}

	public virtual void Lose(GameObject other)
	{
		knownIDs.Remove(other.GetInstanceID());
	}

	public virtual void Perceive(GameObject other)
	{
		Debug.DrawLine(transform.position, other.transform.position, Color.red, 1f);
		knownIDs.Add(other.GetInstanceID());
	}

}