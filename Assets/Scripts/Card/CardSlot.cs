using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public int cardId;
    [SerializeField] Card card;

    UserData userData => GameManager.Instance.userData;

    public virtual void Initialize(int cardId)
    {
        this.cardId = cardId;
        card.Initialize(cardId);
    }

    public virtual void RemoveFromHand()
    {
        userData.RemoveCardFromHand(cardId);
    }    
}
