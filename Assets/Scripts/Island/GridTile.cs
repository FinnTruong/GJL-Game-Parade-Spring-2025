using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public SpriteRenderer sr;
    public Vector3Int coord;
    public int elevation;

    public bool HasObject => currentObject != null;
    public bool IsRootTile;
    public PlaceableObject currentObject;


    public System.Action OnHover;
    public System.Action OnExit;
    public System.Action OnClicked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    public void SetTileCoord(Vector3Int coord)
    {
        this.coord = coord;
    }

    public void ShowTile()
    {
        sr.color = Color.white;
    }

    public void HideTile()
    {
        sr.color = Color.clear;
    }

    public void SetObject(PlaceableObject gridObj, bool isRootTile = false)
    {
        currentObject = gridObj;
        IsRootTile = isRootTile;
        //currentObject = gridObj;
    }

    public void RemoveObject()
    {
        currentObject = null;
        IsRootTile = false;
    }

    private void OnMouseEnter()
    {
        OnHover?.Invoke();
    }

    private void OnMouseExit()
    {
        OnExit?.Invoke();
    }

    private void OnMouseUpAsButton()
    {
        OnClicked?.Invoke();
    }
}
