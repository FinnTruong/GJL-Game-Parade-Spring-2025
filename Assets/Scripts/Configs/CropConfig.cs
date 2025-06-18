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
    public int cardId;
    public CropType cropType;
    public GameObject cropPrefab;
    public List<CropRewardData> rewardData;
}


[CreateAssetMenu(fileName = "CropConfig", menuName = "Config/CropConfig")]
public class CropConfig : ScriptableObject
{
    public List<CropConfigData> Collection;
}
