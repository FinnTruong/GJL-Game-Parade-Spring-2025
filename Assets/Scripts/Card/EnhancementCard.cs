using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnhancementCard : Card
{
    public UnityEvent onStartDrag;
    public UnityEvent onEndDrag;

    PlaceableObject targetObject;

    GridTile rootTile;
    PlacementSystem placementSystem => PlacementSystem.Instance;
    InputManager inputManager => InputManager.Instance;

    public override void Initialize(CardType id)
    {
        base.Initialize(id);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        onStartDrag?.Invoke();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        var hoverTile = inputManager.GetSelectedTile();
        if (hoverTile == null)
        {
            targetObject = null;
        }
        else if(hoverTile.currentObject == null)
        {
            targetObject = null;
        }    
        else
        {
            targetObject = hoverTile.currentObject;
        }
        //icon.gameObject.SetActive(rootTile == null);
        cardVisual.gameObject.SetActive(targetObject == null);

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        onEndDrag?.Invoke();

        if (targetObject != null)
        {

            OnDeleteCard?.Invoke(this);
        }

    }
}
