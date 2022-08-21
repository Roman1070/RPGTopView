using UnityEngine;

public class Panel : ViewsContainer
{
    [SerializeField] private GroupOpacityAnimation _animation;
    [SerializeField] private CanvasGroup _canvasGroup;
    public bool IsActive { get; private set; }

    private void Awake()
    {
        IsActive = _canvasGroup.alpha > 0;
    }

    public void SetActive(bool value, bool immediate = false)
    {
        if (IsActive == value) return;

        if (immediate)
             _canvasGroup.alpha = value ? 1 : 0;
        else
            _animation.Play(0, null, !value);
        
        _canvasGroup.blocksRaycasts = value;
        IsActive = value;
    }
}
