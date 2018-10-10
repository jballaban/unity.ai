using UnityEngine;
using System.Collections.Generic;

public class IntegrationSceneSetup : MonoBehaviour
{
    CameraControl cameraControl;
    public int scenario = 1;
    public static string PREFAB_PERSON = "Person";

    void Awake()
    {
        cameraControl = GameObject.FindObjectOfType<CameraControl>();
    }

    void Start()
    {
        InitScenario();
    }

    void InitScenario()
    {
        Debug.Log("Setup:InitScenario:" + scenario);
        WorldManager.instance.instantiateEventHandler.AddListener(OnWorldManagerInstantiate);
        switch (scenario)
        {
            case 1:
                for (var i = 0; i < 100; i++)
                    WorldManager.instance.Instantiate(PREFAB_PERSON, new Vector3(Random.Range(0, 50), 1, Random.Range(0, 50)));
                break;
        }
    }

    void OnWorldManagerInstantiate(GameObject obj)
    {
        if (cameraControl)
            cameraControl.m_Targets.Add(obj.transform);
    }
}