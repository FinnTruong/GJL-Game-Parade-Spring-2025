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
        int id = Random.Range(0, 6);
        userData.AddCardToHand(id);
    }
}
