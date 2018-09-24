using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class SensoryTests
{
    GameObject EyesPrefab()
    {
        var eyes = new GameObject("eyes");
        var collider = eyes.AddComponent<SphereCollider>();
        collider.radius = 5f;
        collider.isTrigger = true;
        return eyes;
    }

    GameObject PersonPrefab()
    {
        var person = new GameObject("person");
        person.AddComponent<Rigidbody>();
        person.AddComponent<BoxCollider>();
        var eyes = EyesPrefab();
        var neweyes = GameObject.Instantiate(eyes, Vector3.forward + Vector3.up, Quaternion.identity, person.transform);
        neweyes.name = eyes.name;
        return person;
    }

    Dictionary<string, GameObject> Scenario1()
    {
        var scene = new Dictionary<string, GameObject>();
        scene["self"] = GameObject.Instantiate(PersonPrefab(), new Vector3(10, 10, 10), Quaternion.identity);
        scene["self"].transform.name += ": self";
        scene["self"].transform.Find("eyes").gameObject.AddComponent<SimpleVisionSensorComponent>();
        scene["enemy"] = GameObject.Instantiate(PersonPrefab(), new Vector3(20, 10, 10), Quaternion.identity);
        scene["enemy"].transform.name += ": enemy";
        return scene;
    }

    [UnityTest]
    public IEnumerator VisionTests()
    {
        var scenario = Scenario1();
        var vision = scenario["self"].transform.Find("eyes").GetComponent<SimpleVisionSensorComponent>();
        Assert.AreEqual(0, vision.knownIDs.Count);
        // move enemy close enough to see
        scenario["enemy"].transform.position = scenario["self"].transform.position;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(1, vision.knownIDs.Count);
    }
}