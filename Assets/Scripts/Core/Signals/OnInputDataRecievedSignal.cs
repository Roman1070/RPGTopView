using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OnInputDataRecievedSignal : ISignal
{
    public InputDataPack Data;

    public OnInputDataRecievedSignal(InputDataPack data)
    {
        Data = data;
    }

}
