using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
    public static Color DEFAULT_COLOR = new Color(1, 1, 1, 1);
    public static Color PALE_RED = new Color(255, 112 , 102, 255) / 255f;
}

public struct PlayerPrefsKey
{
    public const string GOLD_LEAF = "GOLD_LEAF";
    public const string XP = "XP";
}

public enum ResourceType
{
    None = -1,
    GoldLeaf = 0,
    Xp = 1,
}

public enum CornerType
{
    None,
    Top,
    Bottom,
    Left,
    Right,
}

public enum FlavourType
{

}

public enum SeasonType
{

}

public enum DraggedDirection
{
    Up,
    Down,
    Left,
    Right,
}




public enum ResearchType
{
    None = 0,
    Blueberry = 1,
    Lychee = 2,
    Pollination = 3,
    Strategist = 4,
    Crossbreeding = 5,
    LimitBreak = 6,
    GeneticEvolution = 7,
    Expedition = 8,
}

public enum CropType
{
    None = 0,
    Lemon = 1,
    Blueberry = 2,
    Lychee = 3,
    LavenderPear = 4,
    RubyLime = 5,
    ForestGrassApple = 6,
}


