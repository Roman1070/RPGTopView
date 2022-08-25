using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class OnGameStartedSignal : ISignal { }
public class MainMenuServicesLoader : MonoBehaviour
{
    #region DEPENDENCIES
    [Inject]
    protected readonly SignalBus _signalBus;
    [Inject]
    protected readonly CameraMovementConfig _cameraConfig;
    [Inject]
    protected readonly InputConfig _inputConfig;
    #endregion

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
        _signalBus.Subscribe<OnGameStartedSignal>(OnGameStarted, this);
    }

    private void OnGameStarted(OnGameStartedSignal obj)
    {

    }
}
