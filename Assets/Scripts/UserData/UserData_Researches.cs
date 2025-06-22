using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public partial class UserData
{
    public Action OnResearchAdded;

    public List<ResearchType> researches = new();

    public Action OnExpeditionUnlock;

    public void AddResearch(ResearchType type)
    {
        if (researches.Contains(type))
            return;

        researches.Add(type);
        OnResearchAdded?.Invoke();

        switch (type)
        {
            case ResearchType.None:
                break;
            case ResearchType.Blueberry:
                UnlockCard(CardType.Blueberry);
                break;
            case ResearchType.Lychee:
                UnlockCard(CardType.Lychee);
                break;
            case ResearchType.Pollination:                
                break;
            case ResearchType.Strategist:
                MaxHandSize = 10;
                break;
            case ResearchType.Crossbreeding:
                break;
            case ResearchType.LimitBreak:
                MaxCropGeneration = 4;
                break;
            case ResearchType.GeneticEvolution:
                StartCropGeneration = 1;
                break;
            case ResearchType.Expedition:
                OnExpeditionUnlock?.Invoke();
                break;
            default:
                break;
        }
    }   

}
