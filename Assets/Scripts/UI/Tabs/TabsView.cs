using UnityEngine;

public class TabsView : View
{
    public TabMapping[] TabMappings => _tabMappings;

    [SerializeField]
    private TabMapping[] _tabMappings;
    [SerializeField]
    private int _defaultTabIndex;

    private int _activeTabIndex;

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
        _tabMappings[_activeTabIndex].Tab.SetActive(false);
        _tabMappings[_activeTabIndex].Content.gameObject.SetActive(false);

        _tabMappings[index].Tab.SetActive(true);
        _tabMappings[index].Content.gameObject.SetActive(true);

        _activeTabIndex = index;
    }
}
