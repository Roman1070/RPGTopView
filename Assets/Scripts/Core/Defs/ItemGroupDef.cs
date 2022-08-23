using System;

public enum ItemGroup
{
    Resource,
    Weapon,
    Gear
}
[Serializable]
public class ItemGroupDef : Def
{
    public ItemGroup Group;
    public WeaponType WeaponType;
}
