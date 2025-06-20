using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum CardType
{
    None = 0,

    //Crop 
    Lemon = 1000,
    Blueberry = 1001,
    Lychee = 1002,
    LavenderPear = 1003,
    RubyLime = 1004,
    ForestGrassApple = 1005,

    //Enhancement
    Irrigation = 2000,   
    
}


[System.Serializable]
public class CardConfigData
{
    public CardType id;
    public string name;
    [PreviewField]
    public Sprite icon;
    public PlaceableObject prefab;
}


[CreateAssetMenu(fileName = "CardConfig", menuName = "Config/CardConfig")]
public class CardConfig : ScriptableObject
{
    [ListDrawerSettings(ShowItemCount = true, ShowFoldout = true, ListElementLabelName = "id")]
    [Searchable]
    public List<CardConfigData> Collection;

    public CardConfigData GetConfig(CardType id)
    {
        for (int i = 0; i < Collection.Count; i++)
        {
            if (Collection[i].id == id)
                return Collection[i];
        }
        return null;
    }


    public bool IsCropCard(int id) => id >= 1000 && id < 2000;
    public bool IsEnhancementCard(int id) => id >= 2000;
}
