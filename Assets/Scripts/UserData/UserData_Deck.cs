using System;
using System.Collections.Generic;
using UnityEngine;



public partial class UserData
{
    public Action<CardType> OnCardAddedToHand;
    public Action OnHandChanged;

    public List<CardType> CurrentHand = new();

    public List<CardType> AvailableCards = new() { CardType.Lemon };

    public float DrawCardPrice = 2f;
    public int MaxHandSize = 5;

    public bool IsHandFull => CurrentHand.Count >= MaxHandSize;

    public void AddCardToHand(CardType id)
    {
        CurrentHand.Add(id);
        OnCardAddedToHand?.Invoke(id);
        OnHandChanged?.Invoke();
    }

    public void RemoveCardFromHand(CardType id)
    {
        if (CurrentHand.Contains(id))
        {
            CurrentHand.Remove(id);
            OnHandChanged?.Invoke();
        }
    }

    public void UnlockCard(CardType cardType)
    {
        if (!AvailableCards.Contains(cardType))
            AvailableCards.Add(cardType);
    }
}
