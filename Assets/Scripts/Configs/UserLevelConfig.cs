using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct UserLevelConfigData
{
    public int level;
    public string title;
    public int requiredXp;
}

[CreateAssetMenu(fileName = "UserLevelConfig", menuName = "Config/UserLevelConfig")]
public class UserLevelConfig : ScriptableObject
{
    public List<UserLevelConfigData> Collection;

    public UserLevelConfigData GetConfig(int level)
    {
        for (int i = 0; i < Collection.Count; i++)
        {
            if (Collection[i].level == level)
                return Collection[i];
        }
        return Collection[0];
    }

    public int GetLevelFromXp(int xp)
    {
        for (int i = Collection.Count - 1; i >= 0; i--)
        {
            if(xp >= Collection[i].requiredXp)
                return Collection[i].level;
        }

        return 1;
    }

    public int GetLevelRequiredXp(int level)
    {
        for (int i = 0; i < Collection.Count; i++)
        {
            if (Collection[i].level == level)
                return Collection[i].requiredXp;
        }
        return 0;
    }

    public Vector2 GetLevelXpRange(int level)
    {
        var levelCf = GetConfig(level);
        var nextLevelCf = GetConfig(level + 1);
        return new Vector2(levelCf.requiredXp, nextLevelCf.requiredXp);
    }
}
