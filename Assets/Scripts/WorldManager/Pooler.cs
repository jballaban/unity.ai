using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pooler
{
	Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
	Dictionary<string, List<GameObject>> active = new Dictionary<string, List<GameObject>>();
	Dictionary<string, Stack<GameObject>> inactive = new Dictionary<string, Stack<GameObject>>();

	public void AddPrefab(string key, GameObject obj)
	{
		prefabs.Add(key, obj);
		active.Add(key, new List<GameObject>());
		inactive.Add(key, new Stack<GameObject>());
	}

	public void Destroy(string key, GameObject obj)
	{
		active[key].Remove(obj);
		obj.SetActive(false);
		inactive[key].Push(obj);
	}

	public GameObject Instantiate(string key, Transform parent, Vector3? position, Quaternion? rotation)
	{
		GameObject obj;
		if (inactive[key].Count > 0)
		{
			obj = inactive[key].Pop();
			obj.transform.position = position ?? prefabs[key].transform.position;
			obj.transform.rotation = rotation ?? prefabs[key].transform.rotation;
			obj.transform.SetParent(parent);
			obj.SetActive(true);
		}
		else
			obj = GameObject.Instantiate(prefabs[key], position ?? prefabs[key].transform.position, rotation ?? prefabs[key].transform.rotation, parent);
		active[key].Add(obj);
		return obj;
	}

	/// Replace this with a K-D tree or AABB or something more efficient than brute force
	public IEnumerable<GameObject> GetNearby(Vector3 position, float radius)
	{
		var radiusSquared = radius * radius;
		return active.SelectMany(x => x.Value).Where(x => (x.transform.position - position).sqrMagnitude < radiusSquared);
	}

}