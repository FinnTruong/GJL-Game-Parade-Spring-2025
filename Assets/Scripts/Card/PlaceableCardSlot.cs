using UnityEngine;

public class PlaceableCardSlot : CardSlot
{
    [SerializeField] PlaceableCard card;
    CardConfig cardConfig => ConfigManager.Instance.cardConfig;
    public override void Initialize(int cardId)
    {
        base.Initialize(cardId);
        var cf = cardConfig.GetConfig(cardId);
        if (cf != null)
        {
            card.objectPrefab = cf.prefab;
        }
    }
}
