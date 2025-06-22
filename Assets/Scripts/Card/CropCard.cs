using UnityEngine;

public class CropCard : PlaceableCard
{
    UserData userData => GameManager.Instance.userData;
    CropConfig cropConfig => ConfigManager.Instance.cropConfig;
    protected override void TryCombine(Card card)
    {
        base.TryCombine(card);

        if (card.cardID == cardID)
        {
            TryEvolve(card);
        }
        else
        {
            TryCrossbreed(card);
        }



    }

    private void TryEvolve(Card card)
    {
        if (!userData.researches.Contains(ResearchType.Pollination))
            return;
        if(card.Generation == Generation && card.Generation < userData.MaxCropGeneration)
        {
            card.Evolve();
            OnDeleteCard?.Invoke(this,0.25f);
            card.cardVisual.PlayFlashVFX();
            AudioManager.Instance.PlaySFX("Evolve",0.5f);
        }
    }

    private void TryCrossbreed(Card card)
    {
        if (!userData.researches.Contains(ResearchType.Crossbreeding))
            return;
        var crossbredResult = cropConfig.GetCrossbreedingResult(cardID, card.cardID);
        if (crossbredResult != CardType.None)
        {
            Debug.Log("Crossbred Result: " + crossbredResult.ToString());
            int generation = Mathf.RoundToInt((Generation + card.Generation) / 2);
            card.Initialize(crossbredResult);
            card.SetGeneration(generation);
            card.cardVisual.PlayFlashVFX();
            OnDeleteCard?.Invoke(this,0.25f);
            AudioManager.Instance.PlaySFX("Crossbreed",0.5f);
        }
    }


}
