using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [SerializeField] PlaceableCard card;

    CardConfig cardConfig => ConfigManager.Instance.cardConfig;

    public virtual void Initialize(int cardId)
    {
        var cf = cardConfig.GetConfig(cardId);
        if(cf != null)
        {
            card.objectPrefab = cf.prefab;
        }
    }
}
