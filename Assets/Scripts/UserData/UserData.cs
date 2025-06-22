using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UserData
{
    public System.Action OnXpChanged;
    public System.Action OnLevelChanged;

    UserLevelConfig userLevelConfig => ConfigManager.Instance.userLevelConfig;

    private int xp;
    public int Xp
    {
        get => PlayerPrefs.GetInt(PlayerPrefsKey.XP, 0);
        set
        {
            var lastXpValue = xp;
            xp = value;
            OnXpChanged?.Invoke();
            PlayerPrefs.SetInt(PlayerPrefsKey.XP, value);
            var currentLevel = GetLevelFromXp(lastXpValue);
            var newLevel = GetLevelFromXp(value);
            if (newLevel > currentLevel)
            {
                OnLevelChanged?.Invoke();
            }


        }
    }

    public int Level => userLevelConfig.GetLevelFromXp(xp);
    public Vector2 CurrentXpRange => userLevelConfig.GetLevelXpRange(Level);


    public int GetLevelFromXp(int xp)
    {
        return userLevelConfig.GetLevelFromXp(xp);
    }

    public void Load()
    {

    }
}
