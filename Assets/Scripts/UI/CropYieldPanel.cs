using UnityEngine;
using TMPro;

public class CropYieldPanel : GlobalReference<CropYieldPanel>
{
    [SerializeField] TMP_Text leafYieldText;
    [SerializeField] TMP_Text xpYieldText;
    [SerializeField] TMP_Text durationText;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowPanel(Crop crop)
    {
        gameObject.SetActive(true);
        transform.position = crop.transform.position + Vector3.up * 1.1f;
        leafYieldText.SetText($"{Utility.MoneyToString(crop.LeafYield)}");
        xpYieldText.SetText($"{Utility.MoneyToString(crop.XpYield)}");
        durationText.SetText($"{Utility.TimeToString(crop.Duration)}");
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
