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

    public override float InteractionTime => 1;

    public override void OnInteractingFinished()
    {
        base.OnInteractingFinished();
        transform.parent.gameObject.SetActive(false);
    }
}
