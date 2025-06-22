using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



[System.Serializable]
public struct ResearchConfigData
{
    public int requiredLevel;
    [TextArea] public string title;
    [TextArea] public string description;
    [PreviewField] public Sprite icon;
    public int unlockPrice;
    public Color pinColor;
}

[CreateAssetMenu(fileName = "ResearchConfig", menuName = "Config/ResearchConfig")]
public class ResearchConfig : SerializedScriptableObject
{
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    public Dictionary<ResearchType, ResearchConfigData> Collection;

    public ResearchConfigData GetConfig(ResearchType researchType)
    {
        if (Collection.ContainsKey(researchType))
            return Collection[researchType];

        return Collection[0];
    }

    public bool HasRevealed(ResearchType type, int currentLevel)
    {
        if (Collection.ContainsKey(type))
            return Collection[type].requiredLevel >= currentLevel;

        return false;
    }

}
