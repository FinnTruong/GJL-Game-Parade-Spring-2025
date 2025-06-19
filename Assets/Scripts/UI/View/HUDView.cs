using UnityEngine;

public class HUDView : UIView
{
    [SerializeField] CustomUIButton drawCardButton;

    protected override void Awake()
    {
        base.Awake();
        drawCardButton.onClick.AddListener(() => DrawRandomCard());
    }

    public void DrawRandomCard()
    {
        if (userData.CurrentHand.Count > 5)
            return;
        int id = (int)userData.AvailableCards.GetRandomElement();
        userData.AddCardToHand(id);
    }
}
