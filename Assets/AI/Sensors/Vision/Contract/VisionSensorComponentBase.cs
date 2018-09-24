using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class VisionSensorComponentBase : MonoBehaviour
{
    SphereCollider _collider;
    void Start()
    {
        _collider = GetComponent<SphereCollider>();
    }

    public abstract void Lose(GameObject other);

    public abstract void Perceive(GameObject other);

    public virtual bool IsVisible(GameObject target)
    {
        var dir = target.transform.position - transform.position;
        RaycastHit result;
        var wasHit = Physics.Raycast(transform.position, dir, out result, _collider.radius);
        Debug.DrawRay(transform.position, dir, Color.red, 1f);
        return wasHit;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(transform.parent.name + " collides with " + (other.transform.parent ?? other.transform).name);
        if (IsVisible(other.gameObject))
        {
            Debug.Log(transform.parent.name + " sees " + (other.transform.parent ?? other.transform).name);
            Perceive(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Lose(other.gameObject);
    }
}