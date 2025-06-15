using DG.Tweening;
using UIAnimatorCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlaceableInventorySystem : InventorySystem, IPointerExitHandler
{
    [SerializeField] UIAnimator inventoryUIAnimator;
    [SerializeField] GameObject placementMode;
    [SerializeField] Tilemap grid;
    [SerializeField] GameObject dragDetector;
    private void Awake()
    {
        PlaceableObject.GlobalCompletePlacementEvent += DeactivatePlacementMode;
        PlaceableObject.GlobalSetPlacementEvent += ActivatePlacementMode;
    }

    private void OnDestroy()
    {
        PlaceableObject.GlobalCompletePlacementEvent -= DeactivatePlacementMode;
        PlaceableObject.GlobalSetPlacementEvent -= ActivatePlacementMode;
    }

    public void ActivatePlacementMode()
    {
        dragDetector.SetActive(false);
        placementMode.SetActive(true);
        grid.gameObject.SetActive(true);
        if (grid.color != Color.white)
            DOVirtual.Float(0, 1, 0.25f, x => grid.color = new Color(1, 1, 1, x));
        if (inventoryUIAnimator.CurrentAnimType != AnimSetupType.Outro)
        {
            inventoryUIAnimator.PlayAnimation(AnimSetupType.Outro, () =>
            {

            });
        }
    }

    public void DeactivatePlacementMode()
    {
        dragDetector.SetActive(true);
        placementMode.SetActive(false);
        DOVirtual.Float(1, 0, 0.25f, x => grid.color = new Color(1, 1, 1, x))
            .OnComplete(() => grid.gameObject.SetActive(false));
        if (inventoryUIAnimator.CurrentAnimType != AnimSetupType.Intro)
            inventoryUIAnimator.PlayAnimation(AnimSetupType.Intro);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        InventoryItem exitItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (exitItem == null)
            return;
        if (exitItem.CompareTag("InventoryItem"))
        {
            ActivatePlacementMode();
        }
    }
}
