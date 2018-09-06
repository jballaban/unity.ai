using UnityEngine;

public interface ILocationMemory : IMemory
{
    bool Intersects(Vector3 center, float radius);
}