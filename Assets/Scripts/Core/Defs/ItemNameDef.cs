using System;

[Serializable]
public class ItemNameDef : Def
{
    public string Name;

    public ItemNameDef(string name)
    {
        Name = name;
    }
}
