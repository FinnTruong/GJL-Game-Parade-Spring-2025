using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] GameObject[] tileIndicator;
    [SerializeField] GridManager gridManager;

    PlaceableObject currentPreviewObject;        
    Vector2Int currentSize;

    InputManager inputManager => InputManager.Instance;

    Vector3 offset = Vector3.zero;

    public void ShowPreview(PlaceableObject previewObject)
    {
        currentPreviewObject = previewObject;
        rootTile = null;
        float totalTile = previewObject.tileCoords.Count + 1;
        for (int i = 0; i < tileIndicator.Length; i++) 
        {
            tileIndicator[i].SetActive(i < totalTile);
        }
    }

    public void SetPreviewOffset(Vector3 offset) => this.offset = offset;

    public void StopPreview()
    {
        if (currentPreviewObject != null)
            Destroy(currentPreviewObject.gameObject);
        rootTile = null;
        for (int i = 0;i < tileIndicator.Length;i++) 
        {
            tileIndicator[i].SetActive(false);
        }
        
    }

    GridTile rootTile;

    public void UpdatePosition()
    {
        if (!currentPreviewObject)
            return;

        float totalTile = currentPreviewObject.tileCoords.Count + 1;

        if (rootTile != null)
        {
            if(rootTile == inputManager.GetSelectedTile(InputManager.Instance.MouseToWorldPoint() - offset))
            return;
        }    


        rootTile = inputManager.GetSelectedTile(InputManager.Instance.MouseToWorldPoint() - offset);
        if (rootTile != null)
        {            
            var targetTiles = GetPlaceableTiles();
            for (int i = 0; i < totalTile; i++)
            {
                tileIndicator[i].SetActive(true);
                if (i <= targetTiles.Count - 1)
                    tileIndicator[i].transform.position = targetTiles[i].transform.position;
                else
                    tileIndicator[i].SetActive(false);
            }
            currentPreviewObject.gameObject.SetActive(true);
            currentPreviewObject.transform.position = rootTile.transform.position;
        }
        else
        {
            for (int i = 0; i < totalTile; i++)
            {
                tileIndicator[i].SetActive(false);
            }
            currentPreviewObject.transform.position = inputManager.MouseToWorldPoint() - offset;
            //currentPreviewObject.gameObject.SetActive(false);
        }
    }

    private List<GridTile> GetPlaceableTiles()
    {
        List<GridTile> targetTiles = new();
        var rootTile = inputManager.GetSelectedTile(InputManager.Instance.MouseToWorldPoint() - offset);
        if(rootTile != null) 
        {
            targetTiles.Add(rootTile);
            var targetCoord = currentPreviewObject.GetPlacementTileCoord(rootTile.coord);

            foreach (var coord in targetCoord)
            {
                if(gridManager.grid.ContainsKey(coord))
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
