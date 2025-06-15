using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardConfigData
{
    public int id;
    public GameObject prefab;
}


[CreateAssetMenu(fileName = "CardConfig", menuName = "Config/CardConfig")]
public class CardConfig : ScriptableObject
{
    public List<CardConfigData> Collection;

    public CardConfigData GetConfig(int id)
    {
        id = Mathf.Clamp(id, 0, Collection.Count - 1);
        for (int i = 0; i < Collection.Count; i++)
        {
            if (Collection[i].id == id)
                return Collection[i];
        }
        return null;
    }

    public GameObject GetCard(int id)
    {
        var cf = GetConfig(id);
        if (cf != null)
            return cf.prefab;

        return null;
    }


}
