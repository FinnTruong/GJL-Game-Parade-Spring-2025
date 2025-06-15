using System;
using System.Collections.Generic;
using UnityEngine;



public partial class UserData
{
    public Action<int> OnCardAddedToHand;
    public Action OnHandChanged;

    public List<int> CurrentHand = new();


    public void AddCardToHand(int id)
    {
        CurrentHand.Add(id);
        OnCardAddedToHand?.Invoke(id);
    }
}
