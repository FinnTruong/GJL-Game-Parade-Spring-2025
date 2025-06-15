using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

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
