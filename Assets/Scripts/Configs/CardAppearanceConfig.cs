using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public struct CardAppearanceConfigData
{
    public int generation;
    public Color cardColor;
    public Material edgeMaterial;
    public bool hasShine;
}

[CreateAssetMenu(fileName = "CardAppearanceConfig", menuName = "Config/CardAppearanceConfig")]
public class CardAppearanceConfig : ScriptableObject
{
    public List<CardAppearanceConfigData> Collection;

    public CardAppearanceConfigData GetConfig(int generation)
    {
        for (int i = 0; i < Collection.Count; i++)
        {
            if (Collection[i].generation == generation)
                return Collection[i];
        }

        return Collection[0];
    }
}
