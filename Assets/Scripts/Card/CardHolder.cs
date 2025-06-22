using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.Splines;
using Sirenix.OdinInspector;
public class CardHolder : MonoBehaviour
{

    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private CardSlot cropCardSlotPrefab;
    [SerializeField] private CardSlot enhancementCardSlotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField] private int cardsToSpawn = 7;    
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] float swapOffsetThreshold = 1f;
    [SerializeField] float swapDistanceThreshold = 3f;
    [SerializeField] Transform spawnPos;
    public List<Card> cards;
    public List<CardSlot> slots;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    private int CardCount => cards.Count;

    UserData userData => GameManager.Instance.userData;
    CardConfig cardConfig => ConfigManager.Instance.cardConfig;
    CropConfig cropConfig => ConfigManager.Instance.cropConfig;

    private void Awake()
    {
        userData.OnCardAddedToHand += AddCardToHand;
    }

    private void OnDestroy()
    {
        userData.OnCardAddedToHand -= AddCardToHand;
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>().ToList();

        int cardCount = 0;
        UpdateSlotsPosition();
        StartCoroutine(Frame());

        IEnumerator Frame()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardVisual != null)
                {
                    cards[i].cardVisual.UpdateIndex(transform.childCount);
                    cards[i].cardVisual.UpdateTransform();
                }
            }

        }
    }

    private void UpdateSlotsPosition()
    {
        if (cards.Count == 0)
            return;

        float cardSpacing = 1f / Mathf.Clamp(cards.Count, 8, int.MaxValue);
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2f;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < slots.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = (Vector3)spline.EvaluatePosition(p) + splineContainer.transform.position;
            Vector3 dir = spline.EvaluateTangent(p);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            slots[i].transform.DOMove(splinePosition,0.25f);
            slots[i].transform.DORotateQuaternion(rotation, 0.25f);
        }
    }

    public void AddCardToHand(CardType id)
    {
        var cf = cardConfig.GetConfig(id);
        if (cf == null)
            return;

        CardSlot slot = new CardSlot();

        if(cardConfig.IsCropCard((int)id))
        {
            slot = Instantiate(cropCardSlotPrefab, transform);
        }
        else if(cardConfig.IsEnhancementCard((int)id))
        {
            slot = Instantiate(enhancementCardSlotPrefab, transform);
        }

        slot.Initialize(id);
        BindSlotToHand(slot);
        UpdateSlotsPosition();

    }

    Sequence addCardSequence;

    [Button]
    public void BindSlotToHand(CardSlot slot)
    {        
        slot.transform.position = spawnPos.position;
        slot.name += slots.Count - 1;
        slots.Add(slot);
        var card = slot.GetComponentInChildren<Card>();
        cards.Add(card);
        card.PointerEnterEvent.AddListener(CardPointerEnter);
        card.PointerExitEvent.AddListener(CardPointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
        card.OnDeleteCard.AddListener(DeleteCard);
        card.name = CardCount.ToString();

    }

    private void PlayAddCardSequence(Transform newCard)
    {

    }

    private Vector3 GetCardPositionInHand(int index)
    { 
        float cardSpacing = 1f / Mathf.Clamp(cards.Count, 4, int.MaxValue);
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2f;
        float p = firstCardPosition + index * cardSpacing;
        Spline spline = splineContainer.Spline;
        return spline.EvaluatePosition(p);
    }

    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }


    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0,selectedCard.selectionOffset,0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;

    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    void DeleteCard(Card card, float delay = 0f)
    {
        var slot = card.GetComponentInParent<CardSlot>();
        if (card != null && slot != null)
        {
            slot.RemoveFromHand();
            cards.Remove(card);
            slots.Remove(slot);
        }
        Destroy(slot.gameObject);
        DOVirtual.DelayedCall(delay, () => UpdateSlotsPosition());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (hoveredCard != null)
            {
                Destroy(hoveredCard.transform.parent.gameObject);
                cards.Remove(hoveredCard);

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (Card card in cards)
            {
                card.Deselect();
            }
        }

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        //for (int i = 0; i < cards.Count; i++)
        //{
        //    var distance = Vector2.Distance(selectedCard.transform.position, cards[i].transform.position);
        //    if (distance > swapDistanceThreshold)
        //        continue;
            
        //    if (selectedCard.transform.position.x > cards[i].transform.position.x + swapOffsetThreshold)
        //    {
        //        if (selectedCard.ParentIndex() < cards[i].ParentIndex())
        //        {
        //            Swap(i);
        //            break;
        //        }
        //    }

        //    if (selectedCard.transform.position.x < cards[i].transform.position.x - swapOffsetThreshold)
        //    {
        //        if (selectedCard.ParentIndex() > cards[i].ParentIndex())
        //        {
        //            Swap(i);
        //            break;
        //        }
        //    }
        //}
    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        cards[index].transform.localRotation = Quaternion.identity;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);


        //Updated Visual Indexes
        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }

    void Combine()
    {

    }

}
