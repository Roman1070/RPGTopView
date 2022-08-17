using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class CompositionRootHolder : MonoBehaviour
{
    [Inject]
    private SignalBus _signalBus;

    [SerializeField]
    private LoadableService[] ServicesToLoad;

    [EasyButtons.Button]
    private void GatherServices()
    {
        ServicesToLoad = GetComponentsInChildren<LoadableService>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        foreach (var service in ServicesToLoad)
            service.Init();
    }

}
