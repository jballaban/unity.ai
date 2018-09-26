using UnityEngine;

public class Wander : MonoBehaviour
{
	void Update()
	{
		this.transform.Rotate(Vector3.up, Random.Range(-10f, 10f));
		this.transform.Translate(Vector3.forward * Time.deltaTime);
	}
}