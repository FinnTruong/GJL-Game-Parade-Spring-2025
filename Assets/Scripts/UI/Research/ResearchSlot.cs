using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Coffee.UIEffects;

public class ResearchSlot : MonoBehaviour
{
    [SerializeField] ResearchType researchType;
    [SerializeField] GameObject availableRoot;
    [SerializeField] GameObject unavailableRoot;
    [SerializeField] TMP_Text conditionText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text priceText;
    [SerializeField] CustomUIButton unlockButton;
    [SerializeField] GameObject checkmark;
    [SerializeField] UIEffect shineEffect;
    [SerializeField] Image pin;

    UserData userData => GameManager.Instance.userData;
    ResearchConfig researchConfig => ConfigManager.Instance.researchConfig;

    ResearchConfigData cf => researchConfig.GetConfig(researchType);

    private bool HasCompletedResearch => userData.researches.Contains(researchType);


    void Start()
    {
        userData.OnResearchAdded += UpdateUI;
        userData.OnLevelChanged += UpdateUI;
        userData.OnGoldLeafChanged += UpdateUI;

        unlockButton.onClick.RemoveListener(OnPurchaseButtonClicked);
        unlockButton.onClick.AddListener(OnPurchaseButtonClicked);

        UpdateUI();
    }

    private void OnDestroy()
    {
        userData.OnResearchAdded -= UpdateUI;
        userData.OnLevelChanged -= UpdateUI;
        userData.OnGoldLeafChanged -= UpdateUI;
    }


    void UpdateUI()
    {
        availableRoot.SetActive(userData.Level >= cf.requiredLevel || userData.researches.Contains(researchType));
        unavailableRoot.SetActive(userData.Level < cf.requiredLevel && !userData.researches.Contains(researchType));

        titleText.SetText(cf.title);
        descriptionText.SetText(cf.description);
        icon.sprite = cf.icon;

        priceText.SetText($"<sprite=0>{Utility.MoneyToString(cf.unlockPrice)}");
        shineEffect.enabled = userData.GoldLeaf >= cf.unlockPrice;

        unlockButton.gameObject.SetActive(!HasCompletedResearch);
        unlockButton.interactable = userData.GoldLeaf >= cf.unlockPrice;
        checkmark.gameObject.SetActive(HasCompletedResearch);

        var conditionStr = string.Empty;
        for (int i = 0; i < cf.requiredLevel; i++)
        {
            conditionStr += "<sprite=0>";
        }
        conditionText.SetText($"EARN {conditionStr}TO REVEAL");


        pin.color = cf.pinColor;

    }

    void OnPurchaseButtonClicked()
    {
        var price = cf.unlockPrice;
        if(userData.GoldLeaf >= price)
        {
            AudioManager.Instance.PlaySFX("Purchase");
            userData.GoldLeaf -= price;
            userData.AddResearch(researchType);
        }
    }


    
}
