using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UIAnimatorCore;
using UnityEngine;
using UnityEngine.UI;


public class RadialMenu : MonoBehaviour
{
    [SerializeField] UIAnimator uiAnimator;
    [SerializeField] Image menuBar;
    [SerializeField] Image selectionBar;
    [SerializeField] RectTransform maskRect;
    [SerializeField] RectTransform barRect;
    [SerializeField] RectTransform itemRoot;
    [SerializeField] TMP_Text itemTitleText;
    [SerializeField] RectTransform dividerRoot;
    [SerializeField] GameObject dividerPrefab;

    [Header("Params")]

    [SerializeField][Range(0f, 360f)] float menuAngle;
    [SerializeField][Range(0, 10)] float innerRadius;
    [SerializeField][Range(0, 10)] float outerRadius;
    [SerializeField][Range(0, 10)] int itemCount;

    [SerializeField] List<RadialMenuItem> menuItemPrefabList;

    private List<RadialMenuItem> itemList = new();


    private int currentSelectedItem = -100;

    public bool IsOpen => gameObject.activeInHierarchy;

    public System.Action OnOpenMenu;
    public System.Action OnCloseMenu;

    Vector2 lastMousePos;

    private void Awake()
    {
        Refresh();
    }
    public void Refresh()
    {
        SetMenuSize();
        SetSelectionBarSize();
        SetItemPosition();
        SetDivider();
    }

    private void SetMenuSize()
    {
        if (menuBar == null)
            return;
        menuBar.fillMethod = Image.FillMethod.Radial360;
        menuBar.fillAmount = menuAngle / 360f;
        menuBar.transform.eulerAngles = new Vector3(0, 0, menuAngle / 2f);

        barRect.sizeDelta = Vector2.one * outerRadius * 100;
        maskRect.sizeDelta = Vector2.one * innerRadius * 100;
    }

    private void SetItemPosition()
    {
        itemRoot.DestroyAllChildren(true);
        itemList.Clear();

        for (int i = 1; i <= itemCount; i++)
        {
            var item = Instantiate(menuItemPrefabList[Mathf.Clamp(i-1,0,menuItemPrefabList.Count-1)], itemRoot);
            item.OnSelectItem += Close;
            var allocatedAngle = menuAngle / itemCount;
            var startAngle = menuAngle / 2;
            var targetAngle = startAngle - (allocatedAngle * i) + allocatedAngle / 2;
            item.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            item.SetIconRot();
            itemList.Add(item);
        }
    }
    
    private void SetDivider()
    {
        dividerRoot.DestroyAllChildren(true);
        if (itemList.Count <= 1)
            return;
        for (int i = 0; i < itemCount; i++)
        {
            var divider = Instantiate(dividerPrefab, dividerRoot);
            var allocatedAngle = menuAngle / itemCount;
            var startAngle = menuAngle / 2;
            var offsetAngle = itemCount % 2 != 0 ? allocatedAngle : allocatedAngle / 2;
            var targetAngle = startAngle - (allocatedAngle * i) + allocatedAngle / 2 + offsetAngle;
            divider.transform.eulerAngles = new Vector3(0, 0, targetAngle);
        }
    }

    private void SetSelectionBarSize()
    {
        selectionBar.fillAmount = (menuAngle / itemCount) / 360f;
    }

    public void Update()
    {
        if (!IsOpen)
            return;
        if (Vector2.Distance(lastMousePos, Input.mousePosition) <= 10f)
            return;
        lastMousePos = Input.mousePosition;
        GetSelectedItem();
        if(Input.GetMouseButtonDown(0))
        {
            if (currentSelectedItem == -100 && IsOpen && !uiAnimator.IsPlaying)
                Close();
            else if(currentSelectedItem != -100)
            {
                var item = itemList[currentSelectedItem];
                item.Select();
            }    
        }
    }

    private void GetSelectedItem()
    {
        var screenPos = transform.position;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = Vector2.Distance(mousePos, screenPos);
        if (distance < innerRadius - 1 || distance > outerRadius - 1)
        {
            currentSelectedItem = -100;
            selectionBar.gameObject.SetActive(false);
            DeselectAllItem();
            return;
        }
        var radian = (-menuAngle / 2f) * Mathf.Deg2Rad;
        var axis = new Vector2(Mathf.Sin(radian), Mathf.Cos(radian)).normalized;
        var dir = (mousePos - screenPos).normalized;
        float angle = -Vector2.SignedAngle(axis, dir);
        if (angle < 0)
            angle += 360f;
        //Debug.Log(distance);
        int target = (int)(angle * itemCount / menuAngle);
        if (currentSelectedItem != target)
        {
            currentSelectedItem = target;
            if (currentSelectedItem >= 0 && currentSelectedItem <= itemList.Count - 1)
            {
                var item = itemList[currentSelectedItem];
                SwitchItem(item);
            }
            else
            {
                selectionBar.gameObject.SetActive(false);
                currentSelectedItem = -100;
                DeselectAllItem();
            }
        }

    }

    private void SwitchItem(RadialMenuItem item)
    {
        DeselectAllItem();
        item.HighlightItem();
        var allocatedAngle = menuAngle / itemCount;
        var targetAngle = item.transform.eulerAngles + Vector3.forward * (allocatedAngle / 2);
        if (!selectionBar.gameObject.activeInHierarchy)
        {
            selectionBar.gameObject.SetActive(true);
            selectionBar.transform.eulerAngles = targetAngle;
        }
        else
        {
            selectionBar.transform.DOKill();
            selectionBar.transform.DORotate(targetAngle, 0.1f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);
        }
        itemTitleText.gameObject.SetActive(true);
        itemTitleText.SetText($"{item.menuTitle}");
    }    

    private void DeselectAllItem()
    {
        itemTitleText.gameObject.SetActive(false);
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].DeselectItem();
        }
    }

    public void Open()
    {
        if (uiAnimator.IsPlaying)
            return;
        gameObject.SetActive(true);
        OnOpenMenu?.Invoke();
    }

    public void Close()
    {
        if (uiAnimator.IsPlaying)
            return;
        OnCloseMenu?.Invoke();
        uiAnimator.PlayAnimation(AnimSetupType.Outro, () =>
        {
            gameObject.SetActive(false);

        });
    }

}
