using UnityEngine;

public class Wander : MonoBehaviour
{
    Terrain terrain;

    void Start()
    {
        terrain = GameObject.FindObjectOfType<Terrain>();
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up, Random.Range(-10f, 10f));
        this.transform.Translate(Vector3.forward * Time.deltaTime);
        if (this.transform.position.x < 0 || this.transform.position.z < 0 || this.transform.position.x > 500 || this.transform.position.z > 500)
        {
            this.transform.Rotate(Vector3.up, 90);
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, 0, 500), this.transform.position.y, Mathf.Clamp(this.transform.position.z, 0, 500));
        }

    }
}