using System;
using System.Collections.Generic;
using UnityEngine;



public partial class UserData
{
    public Action<CardType> OnCardAddedToHand;
    public Action OnHandChanged;

    public List<CardType> CurrentHand = new();

    public List<CardType> AvailableCards = new() { CardType.Lemon, CardType.Blueberry, CardType.Lychee, CardType.Irrigation };

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
}
