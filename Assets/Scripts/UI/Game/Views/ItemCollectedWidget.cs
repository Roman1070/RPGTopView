using UnityEngine;
using UnityEngine.UI;

public class ItemCollectedWidget : View
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _count;
    [SerializeField] private Text _name;
    [SerializeField] private GroupOpacityAnimation Appearence;
    [SerializeField] private GroupOpacityAnimation Disappearence;
    [SerializeField] private TransformAnimation Transform;

    public void SetItem(Item item, int count)
    {
        _icon.sprite = item.IconDef.Icon;
        _count.text = count.ToString();
        _name.text = item.NameDef.Name;
    }

    public void PlayAnims()
    {
        Transform.SetProgress(0);

        Appearence.Play(0,()=>
        {
            Disappearence.Play();
            Transform.Play();
        });
    }
}
