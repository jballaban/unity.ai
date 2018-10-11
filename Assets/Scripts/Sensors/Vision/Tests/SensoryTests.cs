using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class SensoryTests
{
    [UnityTest]
    public IEnumerator VisionTests()
    {
        var scene = new GameObject("scene");
        var worldmanager = scene.AddComponent<WorldManager>();
        worldmanager.prefabs.Add(Resources.Load<GameObject>(VisionSampleSceneSetup.PREFAB_PERSON));
        worldmanager.Initialize();
        scene.AddComponent<VisionSampleSceneSetup>();
        yield return new WaitForEndOfFrame(); // ensure all setup is complete
        var self = GameObject.Find("self");
        var vision = self.GetComponent<VisionSensorComponent>();
        Assert.AreEqual(0, vision.GetCurrentlyVisible().Count);
        var enemy = GameObject.Find("enemy");
        enemy.transform.position = self.transform.position;
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(1, vision.GetCurrentlyVisible().Count);
    }
}