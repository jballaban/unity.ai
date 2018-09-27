using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour, IVisionObjectProvider
{
    Pooler pooler = new Pooler();
    public const string PREFAB_PERSON = "Person";
    public static WorldManager instance;
    public Transform root;
    CameraControl cameraControl;

    public GameObject Instantiate(string key, Vector3 position, Transform parent = null)
    {
        var obj = pooler.Instantiate(PREFAB_PERSON, parent ?? root, position, rotation: null);
        cameraControl.m_Targets.Add(obj.transform);
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
        Debug.Log("WorldManager:Awake");
        var prefab = Resources.Load<GameObject>(PREFAB_PERSON);
        pooler.AddPrefab(PREFAB_PERSON, prefab);
        instance = this;
        cameraControl = GameObject.FindObjectOfType<CameraControl>();
    }
}