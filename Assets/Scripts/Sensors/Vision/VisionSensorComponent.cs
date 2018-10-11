using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class VisionSensorComponent : MonoBehaviour
{
    public float refreshRate = 1f;
    public float range = 10f;
    public IVisionObjectProvider objectProvider;
    public class PerceiveEvent : UnityEvent<GameObject, GameObject> { }
    public PerceiveEvent PerceiveEventHandler = new PerceiveEvent();
    public bool AutoCheck = true;
    Coroutine _autocheck = null;

    void Awake()
    {
        if (objectProvider == null) objectProvider = InterfaceHelper.FindObject<IVisionObjectProvider>();
    }

    void Update()
    {
        if (AutoCheck && _autocheck == null)
            _autocheck = StartCoroutine(Recheck());
        else if (!AutoCheck && _autocheck != null)
        {
            StopCoroutine(_autocheck);
            _autocheck = null;
        }
    }

    IEnumerator Recheck()
    {
        while (true)
        {
            foreach (var obj in GetCurrentlyVisible())
                PerceiveEventHandler.Invoke(this.gameObject, obj);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public List<GameObject> GetCurrentlyVisible()
    {
        return objectProvider.GetObjectsWithinRange(transform.position, range).Where(x => x != this.gameObject && IsVisible(x)).ToList();
    }

    protected virtual bool IsVisible(GameObject other)
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, range))
            return false;
        return (hit.collider.gameObject == other);
    }

}