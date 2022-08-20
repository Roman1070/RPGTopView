using UnityEngine;
using UnityEngine.UI;

public class CollectButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Text _text;

    public void SetData(string text)
    {
        _text.text = text;
    }
    public void SetActive(bool val)
    {
        _canvasGroup.alpha = val ? 1 : 0;
    }
}
