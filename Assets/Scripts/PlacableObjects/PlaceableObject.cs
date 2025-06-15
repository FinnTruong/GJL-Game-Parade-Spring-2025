using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UIAnimatorCore;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlaceableObject : SerializedMonoBehaviour
{
    public static System.Action GlobalCompletePlacementEvent;
    public static System.Action GlobalSetPlacementEvent;

    public SpriteRenderer sr;
    public List<Vector3Int> tileCoords;

    public GridTile rootTile;
    public List<GridTile> occupiedTiles;

    public System.Action OnCompletePlacementEvent;

    GridManager gridManager;

    public bool hasSetPlacement;

    public bool canChangePosition;

    public GridTile lastTile;
    public Vector3 lastPosition;

    bool canShowPreview;

    protected virtual void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
    }

    public List<Vector3Int> GetPlacementTileCoord(Vector3Int rootTile)
    {
        List<Vector3Int> result = new();
        result.Add(rootTile);

        for (int i = 0; i < tileCoords.Count; i++)
        {
            result.Add(rootTile + tileCoords[i]);
        }
        return result;
    }

    public virtual void OnConfirmPlacement()
    {
        Debug.Log("Confirm Placement");
        GlobalCompletePlacementEvent?.Invoke();
        OnCompletePlacementEvent?.Invoke();
        gridManager.PlaceObjectAt(rootTile.coord, this);
        canChangePosition = false;
        PlacementSystem.Instance.CompletePlacement();
    }

    public virtual void OnCancelPlacement()
    {
        GlobalCompletePlacementEvent?.Invoke();
        OnCompletePlacementEvent?.Invoke();
        transform.DOScale(0, 0.25f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            Destroy(gameObject);
        });
        canChangePosition = false;
        PlacementSystem.Instance.CompletePlacement();
    }

    
    public virtual void OnTouched()
    {

    }
    public virtual void OnClicked()
    {
        Debug.Log("On Clicked");
    }

    public virtual void SetPreview(bool flag)
    {
        if (canShowPreview == flag)
            return;

        canShowPreview = flag;
        Debug.Log("Show Preview: " + canShowPreview);
    }

    public virtual void RemoveObject()
    {
        foreach (var tile in occupiedTiles)
        {
            tile.RemoveObject();
        }
        Destroy(gameObject);
    }

    public virtual void SetHighlight(bool flag)
    {

    }

}
