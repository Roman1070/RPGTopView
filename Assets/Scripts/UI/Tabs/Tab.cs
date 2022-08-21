using UnityEngine;
using UnityEngine.UI;

public class Tab : View
{
    public Button Button;

    [SerializeField] private GameObject ActiveState;
    [SerializeField] private GameObject InactiveState;
    public int Index;

    public void SetIndex(int value)
    {
        Index = value;
    }

    public void SetActive(bool value)
    {
        ActiveState.SetActive(value);
        InactiveState.SetActive(!value);
    }
}