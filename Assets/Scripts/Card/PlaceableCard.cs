using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlaceableCard : Card
{
    public static Action OnFinishPlacement;

    public PlaceableObject objectPrefab;
    public UnityEvent onStartDrag;
    public UnityEvent onEndDrag;

    GridTile rootTile;
    PlaceableObject linkedObject;
    PlacementSystem placementSystem =>PlacementSystem.Instance;
    InputManager inputManager => InputManager.Instance;

    CardConfig cardConfig => ConfigManager.Instance.cardConfig;

    public override void Initialize(CardType cardId)
    {
        base.Initialize(cardId);
        var cf = cardConfig.GetConfig(cardId);
        if (cf != null)
        {
            objectPrefab = cf.prefab;
        }

    }

    //protected override void TryCombine(Card card)
    //{
    //    base.TryCombine(card);
    //    //var crossbredResult = cropConfig.GetCrossbreedingResult(cardID, card.cardID);
    //    //if (crossbredResult != CardType.None)
    //    //{
    //    //    Debug.Log("Crossbred Result: " + crossbredResult.ToString());
    //    //    card.Initialize(crossbredResult);
    //    //    OnDeleteCard?.Invoke(this);
    //    //}
        
    //}

    private void DelinkObject()
    {
        rootTile = null;
        linkedObject.OnCompletePlacementEvent -= DelinkObject;
        linkedObject = null;
    }

    #region Drag Logic
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (linkedObject == null)
            linkedObject = Instantiate(objectPrefab);
        linkedObject.InitData(this);
        placementSystem.offset = Vector3.zero;
        placementSystem.StartPlacement(linkedObject);
        onStartDrag?.Invoke();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        var hoverTile = inputManager.GetSelectedTile();
        if (hoverTile == null)
        {
            rootTile = null;
        }
        else if (hoverTile.currentObject != null)
        {
            rootTile = null;
        }
        else
        {
            rootTile = hoverTile;
        }
        //icon.gameObject.SetActive(rootTile == null);
        linkedObject.SetPreview(rootTile != null);
        linkedObject.gameObject.SetActive(rootTile != null);
        cardVisual.gameObject.SetActive(rootTile == null);

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (linkedObject.rootTile == null || rootTile == null)
        {
            linkedObject.OnCancelPlacement();
            DelinkObject();
        }
        else
        {
            linkedObject.OnConfirmPlacement();
            linkedObject.OnCompletePlacementEvent += DelinkObject;
            OnDeleteCard?.Invoke(this,0f);
        }

        if (rootTile != null)
        {

        }
        onEndDrag?.Invoke();

    }
    #endregion

}
