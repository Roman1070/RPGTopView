using System;
using UnityEngine;

[Serializable]
public struct EnumerableItem
{
    public string Id;
    public int Count;
}

public class CollectableObject : MonoBehaviour
{
    public EnumerableItem[] Items;
}
