using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] Image xpSlider;
    UserData userData => GameManager.Instance.userData;
    UserLevelConfig userLevelConfig => ConfigManager.Instance.userLevelConfig;

    private void Start()
    {
        userData.OnXpChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        userData.OnXpChanged -= Refresh;
    }


    private void Refresh()
    {
        titleText.SetText($"{userLevelConfig.GetConfig(userData.Level).title}");
        xpSlider.fillAmount = Mathf.InverseLerp(userData.CurrentXpRange.x, userData.CurrentXpRange.y, userData.Xp);
    }
}
