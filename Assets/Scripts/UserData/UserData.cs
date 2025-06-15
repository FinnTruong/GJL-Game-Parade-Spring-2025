using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UserData
{
    public System.Action OnToolChanged;

    public System.Action OnWaterChanged;

    public int Water
    {
        get => PlayerPrefs.GetInt(PlayerPrefsKey.CURRENT_WATER, 1000);
        set
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.CURRENT_WATER, value);
            OnWaterChanged?.Invoke();
        }
    }

    public int WaterCapacity
    {
        get => PlayerPrefs.GetInt(PlayerPrefsKey.RESERVOIR_CAPACITY, 1000);
    }

    private ToolType currentTool;
    public ToolType CurrentTool
    {
        get
        {
            return currentTool;
        }
        set
        {
            currentTool = value;
            OnToolChanged?.Invoke();
        }
    }

    public void Load()
    {

    }
}
