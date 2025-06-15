using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Button button;
    [SerializeField] protected RectTransform bg;

    InventoryItem currentItem;


    protected virtual void Awake()
    {
        button.onClick.AddListener(() => OnClicked());
    }
    public virtual void OnClicked()
    {
        ViewManager.Instance.itemInfoView.Show();
    }

    public virtual void OnItemHover()
    {
        bg.transform.DOKill();
        bg.transform.DOScale(2f, 0.25f).SetEase(Ease.OutQuint);
    }

    public virtual void OnItemExit()
    {
        bg.transform.DOKill();
        bg.transform.DOScale(1f, 0.25f).SetEase(Ease.OutQuint);
    }



    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();
        if (item != null)
        {
            item.currentSlot.OnItemExit();
            item.parentAfterDrag = transform;
            item.currentSlot = this;
            item.currentSlot.OnItemExit();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject enterItem = eventData.pointerDrag;
        if (enterItem == null)
            return;
        if(enterItem.CompareTag("InventoryItem"))
        {
            OnItemHover();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        InventoryItem exitItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (exitItem == null)
            return;
        if (exitItem.CompareTag("InventoryItem") && exitItem.currentSlot != this)
        {
            OnItemExit();
        }
    }

}
