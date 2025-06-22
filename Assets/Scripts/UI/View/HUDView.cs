using TMPro;
using UnityEngine;

public class HUDView : UIView
{
    [SerializeField] TMP_Text drawPriceText;
    [SerializeField] CustomUIButton drawCardButton;

    UserData userData => GameManager.Instance.userData;

    private bool CanDrawCard => !userData.IsHandFull && userData.GoldLeaf >= userData.DrawCardPrice;

    protected override void Awake()
    {
        base.Awake();
        drawCardButton.onClick.AddListener(() => DrawRandomCard());
    }

    private void Start()
    {
        userData.OnGoldLeafChanged += UpdateUI;
        userData.OnHandChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        userData.OnGoldLeafChanged -= UpdateUI;
        userData.OnHandChanged -= UpdateUI;
    }

    public void DrawRandomCard()
    {
        if (userData.CurrentHand.Count >= userData.MaxHandSize)
            return;

        if (userData.GoldLeaf < userData.DrawCardPrice)
            return;

        AudioManager.Instance.PlaySFX("DrawCard");
        userData.GoldLeaf -= userData.DrawCardPrice;
        CardType id = userData.AvailableCards.GetRandomElement();
        userData.AddCardToHand(id);
        UpdateUI();
    }

    private void UpdateUI()
    {
        drawCardButton.interactable = CanDrawCard;


        if (userData.IsHandFull)
        {
            drawPriceText.SetText("MAX");
            drawPriceText.color = GameDefine.DEFAULT_COLOR;
        }
        else
        {
            drawPriceText.SetText($"<sprite=0>{Utility.MoneyToString(userData.DrawCardPrice)}");
            drawPriceText.color = CanDrawCard ? GameDefine.DEFAULT_COLOR : GameDefine.PALE_RED;
        }

    }
}
