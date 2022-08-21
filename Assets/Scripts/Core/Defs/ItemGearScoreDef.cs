using System;

[Serializable]
public class ItemGearScoreDef : Def
{
    public LevelGearScoreMapping[] Mappings;
}

[Serializable]
public class LevelGearScoreMapping
{
    public int Level;
    public int GearScore;
}
