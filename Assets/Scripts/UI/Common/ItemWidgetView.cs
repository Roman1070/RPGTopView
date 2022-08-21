using UnityEngine;
using UnityEngine.UI;

public class ItemWidgetView : ViewsContainer
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Text _count;
    [SerializeField]
    private Image _rarityGradient;
    [SerializeField]
    private Text _gearScore;
    [SerializeField]
    private GameObject _gearScoreIcon;

    public void SetActive(bool val)
    {
        _canvasGroup.alpha = val ? 1 : 0;
        _canvasGroup.blocksRaycasts = val;
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }

    public void SetCount(int count)
    {
        _count.text = count>1?  count.ToString() : string.Empty;
    }

    public void SetRarity(int rarity)
    {
        _rarityGradient.color = PalleteColorConfig.Instance.GetRawColor($"Rarity/{rarity}");
    }

    public void SetGearScore(int gs)
    {
        _gearScoreIcon.SetActive(gs > 0);
        _gearScore.text = gs>0? gs.ToString() : string.Empty;
    }
}
