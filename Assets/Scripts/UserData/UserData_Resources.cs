using System;
using UnityEngine;

public partial class UserData
{
    public Action OnGoldLeafChanged;

    public float GoldLeaf
    {
        get => PlayerPrefs.GetFloat(PlayerPrefsKey.GOLD_LEAF, 0);
        set
        {
            PlayerPrefs.SetFloat(PlayerPrefsKey.GOLD_LEAF, Mathf.Clamp(value, 0, float.MaxValue));
            OnGoldLeafChanged?.Invoke();
        }
    }
}
