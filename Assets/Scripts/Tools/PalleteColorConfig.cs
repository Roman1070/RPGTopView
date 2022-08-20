using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools;
using Tools.UI;

[CreateAssetMenu(fileName = "PalleteColorConfig", menuName = "Configs/PalleteColorConfig")]
public class PalleteColorConfig : SingletonScriptableObject<PalleteColorConfig>
{
    [SerializeField]
    private List<PalleteColorConfigRecord> _colors;
    private Dictionary<string, PalleteColorConfigRecord> _recordMap = new Dictionary<string, PalleteColorConfigRecord>();
    private Dictionary<string, Color> _colorMap = new Dictionary<string, Color>();
    private bool _initialized;

    private void OnEnable()
    {
        Initialize(true);
    }

    private void Initialize(bool force = false)
    {
        if (_initialized && !force)
            return;
        _initialized = true;
        _colorMap.Clear();
        _recordMap = _colors.ToDictionary(_ => _.RecordKey);
        _colorMap.Clear();
        for (int i = 0; i < _colors.Count; i++)
        {
            var record = _colors[i];
            _colorMap.Add(record.RecordKey, ExtractRawColor(record));
        }
    }

    private Color ExtractRawColor(PalleteColorConfigRecord record)
    {
        var recordOverride = record;
        var recordOverridePath = new List<string>();
        while (recordOverride.UseMapping)
        {
            if (recordOverridePath.Contains(recordOverride.MappingKey))
                break;
            if (!_recordMap.ContainsKey(recordOverride.MappingKey))
                break;
            recordOverridePath.Add(recordOverride.MappingKey);
            recordOverride = _recordMap[recordOverride.MappingKey];
        }
        return recordOverride.Value;
    }

    public Color GetRawColor(string key)
    {
        Initialize();
        if (key == null)
            return Color.magenta;
        if (!_colorMap.ContainsKey(key))
            return Color.magenta;
        return _colorMap[key];

    }

#if UNITY_EDITOR
    public Color ExtractRawColor(string key)
    {
        Initialize();
        var record = _colors.Find(_ => _.RecordKey == key);
        if (record == null)
            return Color.magenta;
        return ExtractRawColor(record);
    }

    public List<string> ExtractKeys()
    {
        return _colors.Select(_ => _.RecordKey).ToList();
    }
#endif
}
