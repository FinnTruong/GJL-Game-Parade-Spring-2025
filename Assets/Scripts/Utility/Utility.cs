using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public static class Utility
{
    public static bool IsPointerOverGameObject(string tag, PointerEventData eventData = null)
    {
        List<RaycastResult> eventSystemRaycastResults = GetEventSystemRaycastResults(eventData);
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaycastResults[index];
            if (curRaysastResult.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    public static GameObject GetGameObjectUnderPointer(string tag, PointerEventData eventData = null)
    {
        List<RaycastResult> eventSystemRaycastResults = GetEventSystemRaycastResults(eventData);
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaycastResults[index];
            if (curRaysastResult.gameObject.CompareTag(tag))
            {
                return curRaysastResult.gameObject;
            }
        }
        return null;
    }

    public static bool IsPointerOverLayerMask(string layer, PointerEventData eventData = null)
    {
        List<RaycastResult> eventSystemRaycastResults = GetEventSystemRaycastResults(eventData);
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaycastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer(layer))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsPointerOverUI(PointerEventData eventData = null)
    {
        return IsPointerOverLayerMask("UI", eventData);
    }

    static List<RaycastResult> GetEventSystemRaycastResults(PointerEventData eventData = null)
    {
        if (eventData == null)
            eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public static GameObject GetRaycastObject(LayerMask layerMask, out GameObject hitObject)
    {
        hitObject = null;
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, float.PositiveInfinity, layerMask);
        if (hits.Length > 0)
        {
            hitObject = hits.OrderByDescending(i => i.collider.transform.position.z).OrderByDescending(j => -j.collider.transform.position.y).FirstOrDefault().collider.gameObject;
            return hitObject;
        }

        return null;
    }

    public static Vector3 MouseToWorldPoint()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
