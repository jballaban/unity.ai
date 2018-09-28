using UnityEngine;
using System.Collections.Generic;

public class IntegrationSceneSetup : MonoBehaviour
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
                for (var i = 0; i < 100; i++)
                    WorldManager.instance.Instantiate(WorldManager.PREFAB_PERSON, new Vector3(Random.Range(0, 50), 1, Random.Range(0, 50)));
                break;
        }
    }
}