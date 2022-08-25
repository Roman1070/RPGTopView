using UnityEngine;

public class RenderSpace : MonoBehaviour
{
    public Transform Model;
    public Transform HandAnchor;

    private void Start()
    {
        transform.position = new Vector3(-225,0,-254);
    }
}
