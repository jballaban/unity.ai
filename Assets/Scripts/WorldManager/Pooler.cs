using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pooler
{
    QuadTree<QuadTreeTransformObject> quadtree = new QuadTree<QuadTreeTransformObject>(100, new Rect(0, 0, 500, 500));
    Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    //Dictionary<string, List<GameObject>> active = new Dictionary<string, List<GameObject>>();
    Dictionary<string, Stack<GameObject>> inactive = new Dictionary<string, Stack<GameObject>>();

    public void AddPrefab(string key, GameObject obj)
    {
        prefabs.Add(key, obj);
        //     active.Add(key, new List<GameObject>());
        inactive.Add(key, new Stack<GameObject>());
    }

    public void Destroy(string key, GameObject obj)
    {
        //   active[key].Remove(obj);
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
        //  active[key].Add(obj);
        quadtree.Insert(new QuadTreeTransformObject(obj.transform));
        return obj;
    }

    public IEnumerable<GameObject> GetNearby(Vector3 position, float radius)
    {
        List<QuadTreeTransformObject> results = new List<QuadTreeTransformObject>();
        quadtree.RetrieveObjectsInAreaNoAlloc(new Rect(position.x - radius, position.z - radius, position.x + radius, position.z + radius), ref results);
        return results.Select(x => x.transform.gameObject);
    }

}