using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public static bool IsPointerOverCard(PointerEventData eventData = null)
    {
        return IsPointerOverGameObject("Card");
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

    #region Converter

    static string[] MoneyType = new string[]
{
        "",
        "K" ,
        "M" ,
        "B" ,
        "T" ,
        "a" ,
        "b",
        "c" ,
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "ư",
        "x",
        "y",
        "z",
        "aa",
        "ab",
        "ac",
        "ad",
        "ae",
        "af",
        "ag",
        "ah",
        "ai",
        "aj",
        "ak",
        "al",
        "am",
        "an",
        "ao",
        "ap",
        "ar",
        "as",
        "at",
        "au",
        "av",
        "aw",
        "ax",
        "ay",
        "az",
        "ba",
        "bb",

};
    public static string MoneyToString(this float money)
    {
        if (money < 10000)
        {
            int newMoney = (int)money;
            return newMoney.ToString("#,##0");
        }
        int type = 0;
        float symbolMoney = money;
        float displayMoney = money;

        while (symbolMoney >= 1000)
        {
            symbolMoney = Mathf.Round(symbolMoney / 1000);
            type++;
        }

        displayMoney = Mathf.Round(money * 10 / Mathf.Pow(1000, type));

        long hi = (long)displayMoney / 10;
        long lo = (long)displayMoney % 10;

        var b = new StringBuilder();
        if (type > 0)
        {
            b.Append(hi);
            if (lo != 0)
            {
                b.Append(",");
                //if (lo % 10 != 0)
                //    b.Append(lo.ToString("00"));
                //else
                b.Append(lo.ToString("0"));
            }
        }
        else
        {
            b.Append(money.ToString());
        }
        b.Append(MoneyType[type]);
        return b.ToString();
    }


    public static string MoneyToString(this int money)
    {
        if (money == 0)
            return "FREE";
        var longValue = (float)money;
        return longValue.MoneyToString();
    }

    public static string TimeToString(this float time)
    {
        return $"{Mathf.RoundToInt(time)}s";
    }
    #endregion
    #region
    public static bool ContainsAllItems<T>(this IEnumerable<T> a, IEnumerable<T> b)
    {
        return !b.Except(a).Any();
    }
    #endregion
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
