using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;
using NUnit.Framework.Constraints;

public class GridManager : SerializedMonoBehaviour
{
    public static GridManager Instance;
    private Island island;
    public Tilemap tilemap;
    public Tilemap decorTilemap;
    [SerializeField] GridTile tilePrefab;
    [SerializeField] Transform gridRoot;

    public float zOffset = 3f;

    [Searchable]
    public Dictionary<Vector3Int, GridTile> grid = new();

    public System.Action OnPlaceObject;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    [Button]
    public void GenerateGrid()
    {
        gridRoot.transform.DestroyAllChildren(true);
        grid.Clear();

        BoundsInt bounds = tilemap.cellBounds;
        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);

                    if (tilemap.HasTile(tileLocation))
                    {
                        if (!grid.ContainsKey(new Vector3Int(x, y)))
                        {
                            var gridTile = Instantiate(tilePrefab, gridRoot);
                            gridTile.name = $"Tile [{x},{y}]";
                            var cellWorldPos = tilemap.GetCellCenterWorld(tileLocation);
                            gridTile.SetTileCoord(tileLocation);
                            gridTile.transform.position = new Vector3(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z + zOffset);
                            gridTile.sr.sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                            grid.Add(new Vector3Int(x, y, z), gridTile);
                        }
                    }

                }
            }
        }

    }

    [Button]
    public void GenerateBreakableObjects()
    {

    }    
    public bool HasUpperZTile(Vector3Int coord)
    {
        return grid.ContainsKey(new Vector3Int(coord.x, coord.y, coord.z + 1));
    }

    #region Placement Logic
    public bool CanPlaceObjectAt(Vector3Int rootPos, PlaceableObject objectToPlace)
    {
        List<Vector3Int> positionToOccupy = objectToPlace.GetPlacementTileCoord(rootPos);
        foreach (var pos in positionToOccupy)
        {
            if (!grid.ContainsKey(pos))
                return false;
            if (grid[pos].HasObject)
                return false;
            if (HasUpperZTile(pos))
                return false;
        }
        return true;
    }

    public void PlaceObjectAt(Vector3Int rootPos, PlaceableObject objectToPlace)
    {
        bool validity = CanPlaceObjectAt(rootPos, objectToPlace);
        if (!validity)
            return;

        var targetTilesCoord = objectToPlace.GetPlacementTileCoord(rootPos);
        List<GridTile> targetTiles = new();
        foreach (var tile in targetTilesCoord)
        {
            if (grid.ContainsKey(tile))
            {
                targetTiles.Add(grid[tile]);
                objectToPlace.occupiedTiles.Add(grid[tile]);
            }
        }


        //Remove Decoration
        for (int i = 0; i < targetTiles.Count; i++)
        {
            targetTiles[i].SetObject(objectToPlace, i == 0);
            if (decorTilemap.HasTile(targetTiles[i].coord))
                decorTilemap.SetTile(targetTiles[i].coord, null);
        }

        if(targetTiles.Count > 0) 
        {
            var rootTile = targetTiles[0];
            objectToPlace.transform.SetParent(rootTile.transform);
            objectToPlace.sr.sortingOrder = rootTile.sr.sortingOrder;
            objectToPlace.transform.localPosition = Vector3.zero;

        }

        OnPlaceObject?.Invoke();
        
    }

    public CornerType GetCornerShape(Vector3Int coord)
    {
        var topRightAdjacent = new Vector3Int(coord.x + 1, coord.y, coord.z);
        var bottomLeftAdjacent = new Vector3Int(coord.x - 1, coord.y, coord.z);
        var topLeftAdjacent = new Vector3Int(coord.x, coord.y + 1, coord.z);
        var bottomRightAdjacent = new Vector3Int(coord.x, coord.y - 1, coord.z);

        if (!grid.ContainsKey(topLeftAdjacent) && !grid.ContainsKey(topRightAdjacent) && grid.ContainsKey(bottomLeftAdjacent) && grid.ContainsKey(bottomRightAdjacent))
            return CornerType.Top;

        if (grid.ContainsKey(topLeftAdjacent) && grid.ContainsKey(topRightAdjacent) && !grid.ContainsKey(bottomLeftAdjacent) && !grid.ContainsKey(bottomRightAdjacent))
            return CornerType.Bottom;

        if (!grid.ContainsKey(topLeftAdjacent) && grid.ContainsKey(topRightAdjacent) && !grid.ContainsKey(bottomLeftAdjacent) && grid.ContainsKey(bottomRightAdjacent))
            return CornerType.Left;

        if (grid.ContainsKey(topLeftAdjacent) && !grid.ContainsKey(topRightAdjacent) && grid.ContainsKey(bottomLeftAdjacent) && !grid.ContainsKey(bottomRightAdjacent))
            return CornerType.Right;

        if (IsBottomCorner(coord))
            return CornerType.Bottom;

        return CornerType.None;
    }


    private bool IsBottomCorner(Vector3Int coord)
    {
        foreach (var tile in grid)
        {
            if (tile.Key.x < coord.x)
                return false;

            if (tile.Key.y < coord.y)
                return false;
        }

        return true;
    }

    #endregion
}
