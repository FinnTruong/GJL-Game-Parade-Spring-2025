using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardAppearance : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] Image cardIcon;

    [SerializeField] Image[] tierIndicators;
    [SerializeField] UIEffect shinyEffect;

    CardConfig cardConfig => ConfigManager.Instance.cardConfig;
    CardAppearanceConfig cardAppearanceConfig => ConfigManager.Instance.cardAppearanceConfig;

    public void Initialized(CardType cardId, int generation = 0)
    {
        var cf = cardConfig.GetConfig(cardId);
        var appearanceCf = cardAppearanceConfig.GetConfig(generation);
        if (cf != null)
        {
            titleText.SetText(cf.name);
            cardIcon.sprite = cf.icon;
        }

        for (int i = 0; i < tierIndicators.Length; i++)
        {
            tierIndicators[i].color = appearanceCf.cardColor;
        }

        shinyEffect.enabled = appearanceCf.hasShine;
    }
}
