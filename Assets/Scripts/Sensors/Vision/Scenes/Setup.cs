using UnityEngine;
using System.Collections.Generic;

public class Setup : MonoBehaviour
{
    public int scenario = 1;
    void Start()
    {
        InitScenario();
    }

    void InitScenario()
    {
        Debug.Log("Setup:InitScenario:" + scenario);
        switch (scenario)
        {
            case 1:
                var self = WorldManager.instance.Instantiate(WorldManager.PREFAB_PERSON, new Vector3(10, 1, 10));
                self.name = "self";
                var enemy = WorldManager.instance.Instantiate(WorldManager.PREFAB_PERSON, new Vector3(30, 1, 10));
                enemy.name = "enemy";
                break;
            case 2:
                for (var i = 0; i < 100; i++)
                    WorldManager.instance.Instantiate(WorldManager.PREFAB_PERSON, new Vector3(Random.Range(0, 50), 1, Random.Range(0, 50)));
                break;
        }
    }
}