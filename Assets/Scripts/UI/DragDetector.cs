using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDetector : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] DraggedDirection draggedDirection;
    [SerializeField] UnityEvent onDetectDraggedEvent;

    float horizontalDragDistanceThreshold = Screen.width / 10f;
    float verticalDragDistanceThreshold = Screen.height / 10f;

    public void OnDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 dragVector = (eventData.position - eventData.pressPosition);
        Vector3 dragVectorDirection = dragVector.normalized;
        var dragDir = GetDragDirection(dragVectorDirection);
        var dragThreshold = draggedDirection == DraggedDirection.Up || draggedDirection == DraggedDirection.Down ? verticalDragDistanceThreshold : horizontalDragDistanceThreshold;

        if (dragDir == draggedDirection && dragVector.magnitude >= dragThreshold)
            onDetectDraggedEvent?.Invoke();
    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        return draggedDir;
    }

}
