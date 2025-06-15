using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{

}

public struct PlayerPrefsKey
{
    public const string VERSION_CODE = "VERSION_CODE";
    public const string CURRENT_WATER = "WATER";
    public const string RESERVOIR_CAPACITY = "RESERVOIR_CAPACITY";
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

public enum ToolType
{
    None = 0,
    WateringCan = 1,
    Sickle = 2,
    Inspect = 3,
}


