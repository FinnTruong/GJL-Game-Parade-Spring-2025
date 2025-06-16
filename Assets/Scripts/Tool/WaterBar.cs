using DG.Tweening;
using UnityEngine;

public class WaterBar : MonoBehaviour
{
    //[SerializeField] CustomSlider slider;
    //UserData userData => GameManager.Instance.userData;

    //Tweener tweener;

    //private void Start()
    //{
    //    userData.OnWaterChanged += UpdateBar;
    //    userData.OnToolChanged += UpdateUI;
    //    UpdateUI();
    //}
    //private void OnDestroy()
    //{
    //    userData.OnWaterChanged -= UpdateBar;
    //    userData.OnToolChanged -= UpdateUI;
    //}

    //private void UpdateUI()
    //{
    //    UpdateBar();
    //    if(userData.CurrentTool == ToolType.WateringCan)
    //    {
    //        gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    //private void UpdateBar()
    //{
    //    var currentValue = (float)userData.Water / userData.WaterCapacity;
    //    tweener.Kill();
    //    tweener = DOVirtual.Float(slider.Value, currentValue, 0.1f, x => slider.Value = x).SetEase(Ease.OutQuad);
    //    slider.Value = (float)userData.Water / userData.WaterCapacity;
    //}
}
