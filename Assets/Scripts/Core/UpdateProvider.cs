using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateProvider : MonoBehaviour
{

    public readonly List<Action> Updates = new List<Action>();

    private void Update()
    {
        foreach (var update in Updates)
            update?.Invoke();
    }
}
