using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    None = 0,
    WateringCan = 1,
    Sickle = 2,
    Inspect = 3,
}

[System.Serializable]
public struct ToolConfigData
{
    public string name;
    public string description;
    [PreviewField]
    public Sprite icon;
}


[CreateAssetMenu(fileName = "ToolConfig", menuName = "Config/ToolConfig")]
public class ToolConfig : SerializedScriptableObject
{
    public Dictionary<ToolType, ToolConfigData> Collection;
}
