using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] Image xpSlider;
    [SerializeField] GameObject[] stars;
    UserData userData => GameManager.Instance.userData;
    UserLevelConfig userLevelConfig => ConfigManager.Instance.userLevelConfig;

    private void Start()
    {
        userData.OnXpChanged += Refresh;
        userData.OnLevelChanged += OnLevelChanged;
        Refresh();
        OnLevelChanged();
    }

    private void OnDestroy()
    {
        userData.OnXpChanged -= Refresh;
        userData.OnLevelChanged -= OnLevelChanged;
    }


    private void Refresh()
    {
        titleText.SetText($"{userLevelConfig.GetConfig(userData.Level).title}");
        xpSlider.fillAmount = Mathf.InverseLerp(userData.CurrentXpRange.x, userData.CurrentXpRange.y, userData.Xp);
    }

    private void OnLevelChanged()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i <= userData.Level - 1);
        }
    }
}
