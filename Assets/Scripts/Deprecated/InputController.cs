using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputController : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] SpriteRenderer[] sr;
    [SerializeField] GridObject treePrefab;
    void Start()
    {
        
    }

    void Update()
    {
        CheckInput();
    }


    public RaycastHit2D? GetFocusedOnTile()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);
        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).FirstOrDefault();
        }

        return null;
    }

    public RaycastHit2D? GetFocusedArea(float area = 2)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(mousePos, area, Vector2.zero);
        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).FirstOrDefault();
        }

        return null;
    }

    void CheckInput()
    {
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit.HasValue)
        {
            var gridTile = focusedTileHit.Value.collider.GetComponent<GridTile>();
            GameObject tile = focusedTileHit.Value.collider.gameObject;
            transform.position = tile.transform.position;
            ShowCursor(gridTile != null);
            if(Input.GetMouseButton(0) && gridTile != null && !gridTile.HasObject)
            {
                SpawnObject(treePrefab, gridTile);
            }
            if(Input.GetMouseButton(1) && gridTile != null && gridTile.HasObject)
            {
                ClearObject(gridTile);
            }
        }
        else
        {
            ShowCursor(false);
        }       
    }

    void ShowCursor(bool flag)
    {
        for (int i = 0; i < sr.Length; i++)
        {
            sr[i].gameObject.SetActive(flag);
        }
    }

    public void SpawnObject(GridObject gridObject, GridTile gridTile)
    {
        var tree = Instantiate(gridObject, gridTile.transform);
        tree.sr.sortingOrder = gridTile.sr.sortingOrder;
        tree.transform.position = gridTile.transform.position;
        //gridTile.SetObject(tree);
    }

    public void ClearObject(GridTile gridTile)
    {
        if(gridTile.HasObject)
        {
            var objectToDestroy = gridTile.currentObject.gameObject;
            gridTile.currentObject = null;
            Destroy(objectToDestroy);            
        }
    }


}
