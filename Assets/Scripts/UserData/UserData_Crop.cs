using System.Collections.Generic;
using System;
using UnityEngine;


public partial class UserData
{
    public Dictionary<CropType, int> CropLevelData = new Dictionary<CropType, int>();

    public List<CropType> CropPool = new() { CropType.Lemon };


    public void AddToCropPool(CropType cropType)
    {
        if(!CropPool.Contains(cropType))
            CropPool.Add(cropType);
    }

    public CropType GetRandomCrop()
    {
        if (CropPool.Count > 0)
            return CropPool.GetRandomElement();
        return CropType.Lemon;
    }
}
