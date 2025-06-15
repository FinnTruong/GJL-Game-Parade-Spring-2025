using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class PlaceableItem : InventoryItem
{
    [SerializeField] PlaceableObject cropPrefab;
    public UnityEvent onStartDrag;
    public UnityEvent onEndDrag;

    PlacementSystem placementSystem;
    InputManager inputManager => InputManager.Instance;

    GridTile rootTile;

    PlaceableObject linkedObject;

    public static System.Action OnFinishPlacement;
    protected override void Start()
    {
        base.Start();
        placementSystem = FindFirstObjectByType<PlacementSystem>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (linkedObject == null)
            linkedObject = Instantiate(cropPrefab);
        placementSystem.offset = Vector3.zero;
        placementSystem.StartPlacement(linkedObject);
        onStartDrag?.Invoke();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        var hoverTile = inputManager.GetSelectedTile();
        if(hoverTile == null)
        {
            rootTile = null;
        }
        else if(hoverTile.currentObject != null)
        {
            rootTile = null;
        }
        else
        {
            rootTile = hoverTile;
        }
        icon.gameObject.SetActive(rootTile == null);
        linkedObject.gameObject.SetActive(rootTile != null);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (linkedObject.rootTile == null || rootTile == null)
        {
            linkedObject.OnCancelPlacement();
            DelinkObject();
        }
        else
        {
            linkedObject.OnConfirmPlacement();
            linkedObject.OnCompletePlacementEvent += DelinkObject;
        }

        if(rootTile != null)
        {

        }

        transform.SetParent(parentAfterDrag);
        transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutQuint);
        //transform.localPosition = Vector3.zero;
        IsDragging = false;
        image.raycastTarget = true;
        if (currentSlot != null)
            currentSlot.OnItemExit();
        timer = 0;

        icon.gameObject.SetActive(true);
        onEndDrag?.Invoke();
        onEndDragEvent?.Invoke();
        onEndDragEvent = null;
        //placementSystem.StopPlacement();
    }

    private void DelinkObject()
    {
        rootTile = null;
        linkedObject.OnCompletePlacementEvent -= DelinkObject;
        linkedObject = null;
    }
}
