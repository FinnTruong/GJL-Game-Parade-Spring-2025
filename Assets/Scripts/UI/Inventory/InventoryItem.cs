using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot currentSlot;
    public Image image;
    public Image icon;


    public bool IsDragging;

    Vector3 startPos;

    public Transform parentAfterDrag;

    SwipeController swipeController;
    public float movePageThreshold = 0.55f;
    protected float timer = 0;

    PointerEventData eventData;

    public System.Action onBeginDragEvent;
    public System.Action onEndDragEvent;

    protected virtual void Start()
    {
        swipeController = FindAnyObjectByType<SwipeController>();
        currentSlot = GetComponentInParent<InventorySlot>();
    }

    protected virtual void Update()
    {
        if(IsDragging)
        {
            if (Utility.IsPointerOverGameObject("NextPageZone"))
            {
                timer += Time.deltaTime;
                if (timer > movePageThreshold)
                {
                    timer = 0;
                    swipeController.Next();
                }
            }

            else if (Utility.IsPointerOverGameObject("PreviousPageZone"))
            {
                timer += Time.deltaTime;
                if (timer > movePageThreshold)
                {

                    timer = 0;
                    swipeController.Previous();
                }
            }
            else
                timer = 0;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDragEvent?.Invoke();
        onBeginDragEvent = null;
        parentAfterDrag = transform.parent;
        startPos = transform.position;
        IsDragging = true;
        transform.SetParent(parentAfterDrag.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        timer = 0;

    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        var position = eventData.position;
        transform.position = (Vector3)position;


    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        onEndDragEvent?.Invoke();
        onEndDragEvent = null;
        transform.SetParent(parentAfterDrag);
        transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutQuint);
        //transform.localPosition = Vector3.zero;
        IsDragging = false;
        image.raycastTarget = true;
        if (currentSlot != null)
            currentSlot.OnItemExit();
        timer = 0;

    }

    public virtual void OnRelease()
    {

    }
}
