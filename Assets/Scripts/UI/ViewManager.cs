using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance;

    public HandView hudView;
    public ItemInfoView itemInfoView;
    public PlacementView placementView;
    public ToolView toolView;

    List<UIView> viewList = new();

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        viewList = FindObjectsByType<UIView>(FindObjectsSortMode.None).ToList();
    }

    public void HideAllView()
    {
        foreach (var view in viewList)
        {
            view.Hide();
        }
    }

}
