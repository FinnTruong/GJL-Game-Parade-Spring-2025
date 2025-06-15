using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class PlacementSystem : GlobalReference<PlacementSystem>
{
    [SerializeField] LayerMask placeableLayerMask;
    [SerializeField] GridManager gridManager;
    //[SerializeField] PreviewSystem previewSystem;
    [SerializeField] InputManager inputManager;
    [SerializeField] GameObject[] tileIndicator;


    bool isPlacingObject = false;

    PlaceableObject currentObject;
    GridTile rootTile;

    public Vector3 offset;


    public bool IsPlacingObject => isPlacingObject;
    private void Start()
    {
        gridManager.OnPlaceObject += OnPlaceObject;
    }

    private void OnDestroy()
    {
        gridManager.OnPlaceObject -= OnPlaceObject;
    }
    private void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        if(currentObject != null)
        {
            //if (Input.GetMouseButtonDown(0) && ClickOnCurrentObject())
            //{                
            //    //SetPreviewOffset(Utility.MouseToWorldPoint() - currentObject.transform.position);
            //    //isPlacingObject = true;
            //    currentObject.OnClicked();
            //}
            if (Input.GetMouseButtonUp(0))
            {
                currentObject.OnConfirmPlacement();
                UpdateIndicator();
                isPlacingObject = false;
            }

            if(Input.GetMouseButton(0))
            {                
                UpdatePosition();
            }            
        }
    }

    private bool ClickOnCurrentObject()
    {
        if (Utility.GetRaycastObject(placeableLayerMask, out var hitObject) != null)
        {
            PlaceableObject placeableObject = hitObject.GetComponent<PlaceableObject>();
            if (placeableObject != null)
            {
                if (currentObject == placeableObject)
                    return true;
            }
        }
        return false;
    }

    private PlaceableObject HasClickedOnPlaceableObject()
    {
        if (Utility.GetRaycastObject(placeableLayerMask, out var hitObject) != null)
        {
            PlaceableObject placeableObject = hitObject.GetComponent<PlaceableObject>();
            if (placeableObject != null)
            {
                return placeableObject;
            }
        }
        return null;
    }
    public void StartPlacement(PlaceableObject objectToPlace)
    {
        CompletePlacement();
        if (objectToPlace.rootTile != null)
        {
            objectToPlace.rootTile.IsRootTile = false;
            objectToPlace.rootTile.currentObject = null;
            var targetTiles = GetPlaceableTiles(objectToPlace, rootTile);
            foreach (var tile in targetTiles)
            {
                if (tile.currentObject != null)
                    tile.currentObject = null;
            }

        }
        currentObject = objectToPlace;
        isPlacingObject = true;
        rootTile = null;
        float totalTile = currentObject.tileCoords.Count + 1;
        for (int i = 0; i < tileIndicator.Length; i++)
        {
            tileIndicator[i].SetActive(i < totalTile);
        }

    }

    public void PausePlacement()
    {
        isPlacingObject = false;
    }

    public void CompletePlacement()
    {
        isPlacingObject = false;
        currentObject = null;
        rootTile = null;
    }

    public void OnCompletePlacement()
    {
        for (int i = 0; i < tileIndicator.Length; i++)
        {
            tileIndicator[i].SetActive(false);
        }
    }


    public void SetPreviewOffset(Vector3 offset) => this.offset = offset;


    public void OnPlaceObject()
    {
        for (int i = 0; i < tileIndicator.Length; i++)
        {
            tileIndicator[i].SetActive(false);
        }
    }


    public void UpdatePosition()
    {
        if (!isPlacingObject)
            return;

        if (!currentObject)
            return;

        float totalTile = currentObject.tileCoords.Count + 1;

        if (rootTile != null)
        {
            if (rootTile == inputManager.GetSelectedTile(InputManager.Instance.MouseToWorldPoint() - offset))
                return;
        }


        rootTile = inputManager.GetSelectedTile(InputManager.Instance.MouseToWorldPoint() - offset);
        var targetTiles = GetPlaceableTiles(currentObject, rootTile);
        bool isValid = true;
        foreach (var tile in targetTiles)
        {
            if (tile.currentObject != null)
                isValid = false;
        }
        currentObject.rootTile = rootTile;
        if (rootTile != null && isValid)
        {

            bool canPlaceObject = true;
            foreach (var tile in targetTiles)
            {
                if (tile.currentObject != null)
                    canPlaceObject = false;
            }
            //for (int i = 0; i < totalTile; i++)
            //{
            //    tileIndicator[i].SetActive(true);
            //    if (i <= targetTiles.Count - 1)
            //        tileIndicator[i].transform.position = targetTiles[i].transform.position;
            //    else
            //        tileIndicator[i].SetActive(false);
            //}
            UpdateIndicator();
            currentObject.gameObject.SetActive(true);
            currentObject.transform.position = rootTile.transform.position;
            AudioManager.Instance.PlaySFX("Snap", 0.5f);
        }
        else
        {
            for (int i = 0; i < totalTile; i++)
            {
                tileIndicator[i].SetActive(false);
            }
            currentObject.transform.position = inputManager.MouseToWorldPoint() - offset;
            //currentPreviewObject.gameObject.SetActive(false);
        }

    }

    private void UpdateIndicator()
    {
        if (currentObject == null)
            return;

        float totalTile = currentObject.tileCoords.Count + 1;
        var targetTiles = GetPlaceableTiles(currentObject, currentObject.rootTile);
        for (int i = 0; i < totalTile; i++)
        {
            tileIndicator[i].SetActive(true);
            if (i <= targetTiles.Count - 1)
                tileIndicator[i].transform.position = targetTiles[i].transform.position;
            else
                tileIndicator[i].SetActive(false);
        }
    }


    private List<GridTile> GetPlaceableTiles(PlaceableObject targetObject, GridTile rootTile)
    {
        List<GridTile> targetTiles = new();        
        if (rootTile != null)
        {
            targetTiles.Add(rootTile);
            var targetCoord = targetObject.GetPlacementTileCoord(rootTile.coord);

            foreach (var coord in targetCoord)
            {
                if (gridManager.grid.ContainsKey(coord))
                {
                    var tileToAdd = gridManager.grid[coord];
                    if (!targetTiles.Contains(tileToAdd))
                        targetTiles.Add(tileToAdd);
                }
            }
        }

        return targetTiles;
    }

}
