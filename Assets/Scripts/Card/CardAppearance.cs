using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardAppearance : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] Image cardIcon;

    CardConfig cardConfig => ConfigManager.Instance.cardConfig;
    public void Initialized(CardType cardId)
    {
        var cf = cardConfig.GetConfig(cardId);
        if (cf != null)
        {
            titleText.SetText(cf.name);
            cardIcon.sprite = cf.icon;
        }
    }
}
