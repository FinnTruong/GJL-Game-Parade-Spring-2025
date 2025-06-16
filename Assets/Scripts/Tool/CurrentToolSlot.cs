using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentToolSlot : MonoBehaviour
{
    //[SerializeField] Image toolIcon;
    //[SerializeField] TMP_Text toolNameText;

    //UserData userData => GameManager.Instance.userData;
    //ToolConfig toolConfig => ConfigManager.Instance.toolConfig;


    //void Start()
    //{
    //    userData.OnToolChanged += UpdateUI;
    //    UpdateUI();
    //}

    //private void OnDestroy()
    //{
    //    if (userData != null)
    //        userData.OnToolChanged -= UpdateUI;
    //}

    //private void UpdateUI()
    //{
    //    toolIcon.gameObject.SetActive(true);
    //    switch (userData.CurrentTool)
    //    {
    //        case ToolType.None:
    //            toolNameText.SetText(string.Empty);
    //            toolIcon.gameObject.SetActive(false);
    //            break;
    //        case ToolType.WateringCan:
    //            toolNameText.SetText(toolConfig.Collection[ToolType.WateringCan].name);
    //            toolIcon.sprite = toolConfig.Collection[ToolType.WateringCan].icon;
    //            break;
    //        case ToolType.Sickle:
    //            toolNameText.SetText(string.Empty);
    //            toolIcon.gameObject.SetActive(false);
    //            break;
    //        case ToolType.Inspect:
    //            toolNameText.SetText(string.Empty);
    //            toolIcon.gameObject.SetActive(false);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
