using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WorldManager : MonoBehaviour, IVisionObjectProvider
{
    Pooler pooler = new Pooler();
    public static WorldManager instance;
    public Transform root;
    public InstantiateEvent instantiateEventHandler;
    public class InstantiateEvent : UnityEvent<GameObject> { }
    public List<GameObject> prefabs = new List<GameObject>();
    Dictionary<string, GameObject> prefabLookups = new Dictionary<string, GameObject>();

    public GameObject Instantiate(string key, Vector3 position, Transform parent = null)
    {
        var obj = pooler.Instantiate(key, parent ?? root, position, rotation: null);
        instantiateEventHandler.Invoke(obj);
        return obj;
    }

    public IEnumerable<GameObject> GetObjectsWithinRange(Vector3 position, float radius)
    {
        var matches = pooler.GetNearby(position, radius).ToList();
        var rangeSquared = radius * radius;
        for (int i = 0; i < matches.Count; i++)
        {
            var v = matches[i].transform.position - position;
            if (v.sqrMagnitude > rangeSquared)
                matches.RemoveAt(i--);
        }
        return matches;
    }

    void Awake()
    {
        instantiateEventHandler = new InstantiateEvent();
        foreach (var prefab in prefabs)
        {
            pooler.AddPrefab(prefab.name, prefab);
            prefabLookups[prefab.name] = prefab;
        }
        instance = this;
    }
}