using UnityEngine;

public class QuadTreeTransformObject : IQuadTreeObject
{
    public Transform transform;

    public QuadTreeTransformObject(Transform transform)
    {
        this.transform = transform;
    }

    public Vector2 GetPosition()
    {
        return new Vector3(transform.position.x, transform.position.z);
    }
}