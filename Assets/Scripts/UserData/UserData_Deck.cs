using System;
using System.Collections.Generic;
using UnityEngine;



public partial class UserData
{
    public Action<int> OnCardAddedToHand;
    public Action OnHandChanged;

    public List<int> CurrentHand = new();

    public List<CardType> AvailableCards = new() { CardType.Lemon, CardType.Blueberry, CardType.Lychee, CardType.Irrigation };

    public void AddCardToHand(int id)
    {
        CurrentHand.Add(id);
        OnCardAddedToHand?.Invoke(id);
        OnHandChanged?.Invoke();
    }

    public void RemoveCardFromHand(int id)
    {
        if (CurrentHand.Contains(id))
        {
            CurrentHand.Remove(id);
            OnHandChanged?.Invoke();
        }
    }
}
