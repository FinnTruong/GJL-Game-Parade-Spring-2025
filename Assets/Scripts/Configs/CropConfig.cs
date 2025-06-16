using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CropRewardData
{
    public int level;
    public float goldLeafReward;
    public int xpReward;
}

[Serializable]
public class CropConfigData
{
    public CropType cropType;
    public List<CropRewardData> rewardData;
}


[CreateAssetMenu(fileName = "CropConfig", menuName = "Scriptable Objects/CropConfig")]
public class CropConfig : ScriptableObject
{
    public List<CropConfigData> Collection;
}
