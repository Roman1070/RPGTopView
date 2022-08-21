using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract string Action { get; }
    public bool IsActive { get; protected set; }
    public abstract float InteractionTime { get; }

    private void Awake()
    {
        IsActive = true;
    }

    public virtual void OnInteractingFinished()
    {
        IsActive = false;
    }
}
