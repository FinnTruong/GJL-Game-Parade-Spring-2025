using DG.Tweening;
using UnityEngine;

public class PlaceableInventorySlot : InventorySlot
{
    [SerializeField] GameObject highlight;

    Vector2 startSizeDelta;

    protected override void Awake()
    {
        base.Awake();
        startSizeDelta = bg.sizeDelta;
    }
    public override void OnItemHover()
    {
        bg.transform.DOKill();
        bg.DOSizeDelta(startSizeDelta * 1.5f, 0.2f);
    }

    public override void OnItemExit()
    {
        bg.transform.DOKill();
        bg.DOSizeDelta(startSizeDelta, 0.2f);
    }

    public override void OnClicked()
    {
        
    }
}
