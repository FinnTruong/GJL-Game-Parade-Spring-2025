using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform pagesRoot;

    [SerializeField] float moveDuration;
    [SerializeField] Ease easeType;

    int maxPage;
    int currentPage;
    Vector3 targetPos;
    float dragThreshold;

    [SerializeField] Image[] barImage;

    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    
    private void Start()
    {
        maxPage = pagesRoot.childCount;
        currentPage = 1;
        pagesRoot.anchoredPosition = Vector2.zero;
        targetPos = pagesRoot.localPosition;
        dragThreshold = Mathf.Abs(pageStep.x / 4f);

        UpdateBar();
    }

    [Button]
    public void Next()
    {
        if(currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    [Button]
    public void Previous()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    public void MovePage()
    {
        pagesRoot.transform.DOKill();
        pagesRoot.DOLocalMove(targetPos, moveDuration).SetEase(easeType);
        UpdateBar();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x)
                Previous();
            else
                Next();
        }
        else
            MovePage();
    }

    private void UpdateBar()
    {
        for (int i = 0; i < barImage.Length; i++)
        {
            barImage[i].color = inactiveColor;
        }

        barImage[currentPage - 1].color = activeColor;
    }
}
