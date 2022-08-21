using UnityEngine;

public class TabsView : View
{
    public TabMapping[] TabMappings => _tabMappings;

    [SerializeField]
    private TabMapping[] _tabMappings;
    [SerializeField]
    private int _defaultTabIndex;

    private void Awake()
    {
        for(int i = 0; i < _tabMappings.Length; i++)
        {
            _tabMappings[i].Tab.Button.onClick.RemoveAllListeners();
            int index = i;
            _tabMappings[i].Tab.Button.onClick.AddListener(()=>SetActiveTab(index));
        }
        SetActiveTab(_defaultTabIndex);
    }

    private void SetActiveTab(int index)
    {
        for(int i = 0; i < _tabMappings.Length; i++)
        {
            _tabMappings[i].Tab.SetActive(i==index);
            _tabMappings[i].Content.gameObject.SetActive(i == index);
        }
    }
}
