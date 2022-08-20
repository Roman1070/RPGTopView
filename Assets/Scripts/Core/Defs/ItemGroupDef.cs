using System;

public enum ItemGroup
{
    Resource,
    MeleeWeapon,
    RangedWeapon,
    Gear
}

[Serializable]
public class ItemGroupDef : Def
{
    public ItemGroup Group;
}
