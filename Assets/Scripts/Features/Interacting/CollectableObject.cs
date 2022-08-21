using System;
using UnityEngine;

[Serializable]
public struct EnumerableItem
{
    public string Id;
    public int Count;

    public EnumerableItem(string id, int count)
    {
        Id = id;
        Count = count;
    }
}

public class CollectableObject : InteractableObject
{
    public EnumerableItem[] Items;

    public override string Action => "Collect";

    public override void OnInteractingFinished()
    {
        base.OnInteractingFinished();
        gameObject.SetActive(false);
    }
}
