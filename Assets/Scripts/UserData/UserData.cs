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
        get => xp;
        set
        {
            xp = value;
            OnXpChanged?.Invoke();
        }
    }

    public int Level => userLevelConfig.GetLevelFromXp(xp);
    public Vector2 CurrentXpRange => userLevelConfig.GetLevelXpRange(Level);


    public int GetLevelFromXp(int xp)
    {
        return 1;
    }

    public void Load()
    {

    }
}
