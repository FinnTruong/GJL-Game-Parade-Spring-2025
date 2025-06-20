using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CropRewardData
{
    public int level;
    public float goldLeafReward;
    public int xpReward;
}

[Serializable]
public struct CropConfigData
{    
    public CardType id;
    [TitleGroup("Recipe")]
    public bool hasRecipe;
    [ShowIf(nameof(hasRecipe))]
    public List<CardType> parents;
    [TitleGroup("Data")]
    [ListDrawerSettings(ShowFoldout = true)]
    public List<CropRewardData> rewardData;
}


[CreateAssetMenu(fileName = "CropConfig", menuName = "Config/CropConfig")]
public class CropConfig : SerializedScriptableObject
{
    [Searchable]
    [ListDrawerSettings(ShowFoldout = true, ListElementLabelName = "id")]
    public List<CropConfigData> Collection;

    public CardType GetCrossbreedingResult(CardType parentA, CardType parentB)
    {
        for (int i = 0; i < Collection.Count; i++)
        {
            if (!Collection[i].hasRecipe)
                continue;

            if (Collection[i].parents.Contains(parentA) && Collection[i].parents.Contains(parentB) && parentA != parentB)
                return Collection[i].id;            
        }

        return CardType.None;
    }

}
