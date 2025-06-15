using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PuillTag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float minDragDistance = 0.5f;
    [SerializeField] private float maxDragDistance;
    [SerializeField] private Vector3 axis;

    private Vector3 startPosition;
    private Vector3 startLocalPosition;

    private Vector3 startDraggingOffset;
    private bool isDragging;
    private bool canInteract;
    private Vector3 lastMouseWorldPosition;


    public UnityEvent onPullCompleteEvent;

    private bool pullComplete;

    private void Start()
    {
        startPosition = transform.position;
        startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (!isDragging || !canInteract)
            return;

        Vector3 newPosition = GetClosestPointAlongAxis(Input.mousePosition + startDraggingOffset, axis);

        float newPositionDot = Vector3.Dot((newPosition - startPosition), axis);
        Debug.Log(newPositionDot);
        if (newPositionDot > 0f)
        {
            // Going past the starting position, snap back
            newPosition = startLocalPosition;
            return;
        }

        //if (Vector3.Distance(startPosition, newPosition) > minDragDistance)
        //{
        //    // Dragging too far, snap to max position
        //    transform.DOMove(startPosition + (axis * -1f) * maxDragDistance, 0.5f);
        //    canInteract = false;
        //}


        if (Vector3.Distance(startPosition, newPosition) > maxDragDistance)
        {
            //Dragging too far, snap to max position
            newPosition = startPosition + (axis * -1f) * maxDragDistance;
            canInteract = false;
            pullComplete = true;
        }



        transform.position = newPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        canInteract = true;
        pullComplete = false;
        startDraggingOffset = transform.position - Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        transform.DOKill();
        transform.DOLocalMove(startLocalPosition, 0.2f).SetEase(Ease.OutBack);
        if (pullComplete)
        {
            onPullCompleteEvent?.Invoke();

        }
        pullComplete = false;
        //Snap Back
    }

    Vector3 GetClosestPointAlongAxis(Vector3 point, Vector3 axis)
    {
        // Calculate vector and distance from robot to obstacle (hypotenuse)
        Vector3 toPoint = point - transform.position;
        float toPointDistance = toPoint.magnitude;

        // Calculate theta (angle)
        float theta = Vector3.Angle(axis, toPoint);

        // Using CAH rule (cosine, adjacent, hypotenuse) to find the (adjacent) side length
        float pointIntersectionDistance = Mathf.Cos(theta * Mathf.Deg2Rad) * toPointDistance;

        // Travelling the calculated distance in the direction of the target
        Vector3 intersectionPoint = transform.position + axis * pointIntersectionDistance;

        return intersectionPoint;
    }
}
