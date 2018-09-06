using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class SensoryTests
{

	GameObject PersonPrefab()
	{
		var person = new GameObject("person");
		person.AddComponent<Rigidbody>();
		person.AddComponent<BoxCollider>();
		var eyes = new GameObject("eyes");
		eyes.AddComponent<SphereCollider>().radius = 5f;
		var neweyes = GameObject.Instantiate(eyes, Vector3.forward + Vector3.up, Quaternion.identity, person.transform);
		neweyes.name = eyes.name;
		return person;
	}

	Dictionary<string, GameObject> Scenario1()
	{
		var scene = new Dictionary<string, GameObject>();
		scene["self"] = GameObject.Instantiate(PersonPrefab(), new Vector3(10, 10, 10), Quaternion.identity);
		scene["self"].transform.Find("eyes").gameObject.AddComponent<SimpleVisionSensorComponent>();
		scene["enemy"] = GameObject.Instantiate(PersonPrefab(), new Vector3(20, 10, 10), Quaternion.identity);
		return scene;
	}

	[Test]
	public void VisionTests()
	{
		var scenario = Scenario1();
		var vision = scenario["self"].transform.Find("eyes").GetComponent<SimpleVisionSensorComponent>();
		Assert.AreEqual(0, vision.knownIDs.Count);
		// move enemy close enough to see
		scenario["enemy"].transform.position = scenario["self"].transform.position;
		Assert.AreEqual(1, vision.knownIDs.Count);
	}
}