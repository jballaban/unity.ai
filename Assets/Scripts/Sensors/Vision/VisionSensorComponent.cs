using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisionSensorComponent : MonoBehaviour
{
    public float refreshRate = 1f;
    public float range = 10f;
    public IVisionObjectProvider objectProvider;

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
            foreach (var obj in objectProvider.GetObjectsWithinRange(transform.position, range).ToList())
            {
                if (obj != this.gameObject && IsVisible(obj))
                    Perceive(obj);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    protected virtual bool IsVisible(GameObject other)
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, range))
            return false;
        return (hit.collider.gameObject == other);
    }

    public virtual void Perceive(GameObject other)
    {
        Debug.DrawLine(transform.position, other.transform.position, Color.red, refreshRate);
    }

}