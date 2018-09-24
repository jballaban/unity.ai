using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class VisionSensorComponentBase : MonoBehaviour
{

    public abstract void Lose(GameObject other);

    public abstract void Perceive(GameObject other);

    public virtual bool IsVisible(GameObject target) { return true; }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        Debug.Log(transform.parent.name + " Sees " + other.transform.name);
        if (IsVisible(other.gameObject))
            Perceive(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        Lose(other.gameObject);
    }
}