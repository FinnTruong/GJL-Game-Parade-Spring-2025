using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CropProgressWheelManager : GlobalReference<CropProgressWheelManager>
{
    [SerializeField] CropProgressWheel displayPrefab;
    [SerializeField] CanvasGroup canvasGroup;

    public List<CropProgressWheel> progressWheelList;
    protected UserData userData => GameManager.Instance.userData;


    public void BindProgressWheel(Crop crop)
    {
        for (int i = 0; i < progressWheelList.Count; i++)
        {
            if (progressWheelList[i].IsAvailable)
            {
                progressWheelList[i].Bind(crop);
                return;
            }
        }

        var newProgressWheel = Instantiate(displayPrefab, transform);
        progressWheelList.Add(newProgressWheel);
        newProgressWheel.Bind(crop);
    }
}
