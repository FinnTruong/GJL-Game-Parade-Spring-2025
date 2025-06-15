using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LayerMask placementLayermask;
    [SerializeField] private LayerMask placeableLayerMask;
    [SerializeField] private LayerMask cropLayerMask;

    public System.Action OnClicked, OnExit;
    PlacementSystem placementSystem;

    private GridTile lastTile;
    private PlaceableObject lastObject;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        placementSystem = FindFirstObjectByType<PlacementSystem>();
    }

    private void Update()
    {
        ProcessInput();
    }

    public void ProcessInput()
    {
        if (placementSystem.IsPlacingObject)
            return;

        GetCurrentTargetObject();
        if (Input.GetMouseButtonDown(0))
        {
            if (lastObject != null)
            {
                lastObject.OnClicked();
                return;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (lastObject != null)
            {
                lastObject.OnTouched();
                return;
            }
        }
    }

    private void GetCurrentTargetObject()
    {
        var currentTile = GetSelectedTile();
        if (currentTile == null)
        {
            if (lastObject)
            {
                lastObject.SetHighlight(false);
                lastObject = null;
            }
            lastTile = null;
            return;
        }

        if (currentTile == lastTile)
            return;

        bool hasObject = currentTile.currentObject != null;
        if (hasObject && currentTile != lastTile)
        {
            var placebleObject = currentTile.currentObject;
            //if(Input.GetMouseButton(0))
            //{
            //    placebleObject.OnTouched();
            //}
            //else if(Input.GetMouseButtonDown(0))
            //{
            //    placebleObject.OnClicked();
            //}

            if (placebleObject != lastObject)
            {
                if (lastObject != null)
                    lastObject.SetHighlight(false);
                placebleObject.SetHighlight(true);
            }
        }

        else
        {
            if (lastObject != null)
            {
                lastObject.SetHighlight(false);
            }
        }

        lastTile = currentTile;
        lastObject = currentTile.currentObject ? currentTile.currentObject : null;
    }


    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    public GridTile GetSelectedTile()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return GetSelectedTile(mousePos);
    }

    public GridTile GetSelectedTile(Vector3 pos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero, float.PositiveInfinity, placementLayermask);
        if (hits.Length > 0)
        {
            var hitObject = hits.OrderByDescending(i => i.collider.transform.position.z).FirstOrDefault();
            var targetTile = hitObject.transform.GetComponent<GridTile>();
            if (gridManager.grid.ContainsKey(targetTile.coord + new Vector3Int(0, 0, 1)))
                return null;
            else
                return targetTile;
        }

        return null;
    }

    public Vector3Int GetSelectedTileCoord()
    {
        var targetTile = GetSelectedTile();
        if (targetTile != null)
            return targetTile.coord;
        return new Vector3Int(0, 0, 0);
    }

    public bool HasUpperTile()
    {
        return false;
    }    

    public GameObject GetRaycastObject(LayerMask layerMask, out GameObject hitObject)
    {
        hitObject = null;
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, float.PositiveInfinity, layerMask);
        if (hits.Length > 0)
        {
            hitObject = hits.OrderByDescending(i => i.collider.transform.position.z).OrderByDescending(j=>-j.collider.transform.position.y).FirstOrDefault().collider.gameObject;
            return hitObject;
        }

        return null;
    }

    public Vector3 MouseToWorldPoint()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

}
