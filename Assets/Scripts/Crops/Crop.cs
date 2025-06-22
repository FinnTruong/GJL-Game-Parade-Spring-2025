using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct CropData
{
    
}

public enum CropState
{
    IsGrowing,
    ReadyToHarvest,
}

[Serializable]
public class PlotSpriteData
{
    public Vector3 localPos;
    public Sprite plotSprite;
    public Sprite plotBorderSprite;
}

public class Crop : PlaceableObject
{
    UserData userData => GameManager.Instance.userData;
    CropConfig cropConfig => ConfigManager.Instance.cropConfig;

    public System.Action OnCropMaturedEvent;

    public CropData data;

    [SerializeField] Transform[] productPos;
    [SerializeField] SpriteRenderer plot;
    [SerializeField] SpriteRenderer plotBorder;
    [SerializeField] Dictionary<CornerType, PlotSpriteData> spriteDictionary;
    [SerializeField] Transform crop;
    [SerializeField] Transform sprout;
    [SerializeField] ParticleSystem smokePuffVFX;
    [SerializeField] ParticleSystem sproutVFX;

    public Transform progressWheelPos;

    [SerializeField] Renderer cropRenderer;
    [SerializeField] Material highlightMat;
    [SerializeField] Material normalMat;


    Vector3 plotStartScale;
    Vector3 cropStartScale;
    Vector3 sproutStartScale;

    private bool isPlayingAnimation;
    private bool hasStartedGrowing;
    private float growthTimer;

    CropState currentState;
    CardType cardType;
    int generation;

    private bool IsGrowing => hasStartedGrowing == true && growthTimer < Duration;
    public float GrowthProgress => growthTimer / Duration;

    public float LeafYield => (float)(cropConfig.GetConfig(cardType).leafYield * Math.Pow(5, generation));
    public int XpYield => cropConfig.GetConfig(cardType).xpYield * (generation+1);

    public float Duration = 2f;

    protected override void Awake()
    {
        base.Awake();
        plotStartScale = plot.transform.localScale;
        cropStartScale = crop.transform.localScale;
        sproutStartScale = sprout.transform.localScale;
    }

    public override void InitData(Card card)
    {
        base.InitData(card);
        cardType = card.cardID;
        generation = card.Generation;
        Duration = cropConfig.GetConfig(cardType).matureDuration;
    }

    public override void SetPreview(bool flag) 
    {
        base.SetPreview(flag);
        if (flag)
            CropYieldPanel.Instance.ShowPanel(this);
        else
            CropYieldPanel.Instance.HidePanel();
    }

    public override void OnConfirmPlacement()
    {
        base.OnConfirmPlacement();
        SetPlacement();
        CropYieldPanel.Instance.HidePanel();
    }


    public override void OnCancelPlacement()
    {
        base.OnCancelPlacement();
        CropYieldPanel.Instance.HidePanel();
    }
    public override void OnTouched()
    {
        base.OnTouched();
        Harvest();
    }

    public override void OnClicked()
    {
        base.OnClicked();
        Harvest();
    }

    Sequence spawnFxSequence;
    private void Harvest()
    {
        if (InputManager.Instance.isDraggingCard)
            return;
        if (currentState == CropState.ReadyToHarvest)
        {
            AudioManager.Instance.PlaySFX("Harvest");
            var leafYield = LeafYield;
            userData.GoldLeaf += leafYield;
            userData.Xp += XpYield;
            var spawnLeafPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.25f);
            //var spawnXpPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.25f);
            RewardEffectManager.Instance.SpawnRewardEffect(spawnLeafPos, ResourceType.GoldLeaf, (int)leafYield);
            //RewardEffectManager.Instance.SpawnRewardEffect(spawnXpPos, ResourceType.Xp, (int)XpYield);
            //spawnFxSequence?.Complete();
            //spawnFxSequence = DOTween.Sequence();
            //spawnFxSequence.AppendCallback(() => RewardEffectManager.Instance.SpawnRewardEffect(spawnLeafPos, ResourceType.GoldLeaf, (int)leafYield));
            //spawnFxSequence.AppendInterval(0.25f);
            //spawnFxSequence.AppendCallback(() => RewardEffectManager.Instance.SpawnRewardEffect(spawnLeafPos, ResourceType.Xp, (int)XpYield));
            RemoveObject();
        }
    }

    Sequence cropMaturedSequence;

    public void SetPlacement()
    {
        CornerType cornerType = GridManager.Instance.GetCornerShape(rootTile.coord);
        UpdatePlotShape(cornerType);
        PlayPlantCropSequence();
    }

    Sequence plantCropSequence;
    private void PlayPlantCropSequence()
    {
        plot.gameObject.SetActive(true);
        plot.transform.localScale = Vector3.zero;
        AudioManager.Instance.PlaySFX("Dig");
        sproutVFX.Play();
        sprout.gameObject.SetActive(true);
        sprout.localScale = Vector3.zero;
        plantCropSequence?.Complete();
        plantCropSequence = DOTween.Sequence();
        plantCropSequence.Append(plot.transform.DOScale(plotStartScale, 0.2f).SetEase(Ease.OutQuad));
        cropMaturedSequence.Join(sprout.transform.DOScale(sproutStartScale, 0.35f).SetEase(Ease.OutBack).SetDelay(0.1f));
        plantCropSequence.AppendCallback(() =>
        {
            CropProgressWheelManager.Instance.BindProgressWheel(this);
            StartGrowing();
        });
    }

    private void PlayCropMaturedSequence()
    {
        if (isPlayingAnimation)
            return;
        AudioManager.Instance.PlaySFX("Ding", 0.5f);
        crop.gameObject.SetActive(true);
        crop.localScale = new Vector3(cropStartScale.x, 0, cropStartScale.z);
        cropMaturedSequence?.Complete();
        cropMaturedSequence = DOTween.Sequence();
        cropMaturedSequence.AppendInterval(0.25f);
        cropMaturedSequence.AppendCallback(() =>
        {
            AudioManager.Instance.PlaySFX("Grow", 0.5f);
            sprout.gameObject.SetActive(false);
            smokePuffVFX.Play();
            isPlayingAnimation = true;
        });
        cropMaturedSequence.Join(crop.transform.DOScale(cropStartScale, 0.35f).SetEase(Ease.OutBack).SetDelay(0.1f));
        cropMaturedSequence.AppendCallback(() =>
        {
            isPlayingAnimation = false;

        });

    }

    private void UpdatePlotShape(CornerType cornerType)
    {
        crop.transform.localPosition = spriteDictionary[cornerType].localPos;
        plot.sprite = spriteDictionary[cornerType].plotSprite;
        plotBorder.sprite = spriteDictionary[cornerType].plotBorderSprite;
    }


    public void StartGrowing()
    {
        currentState = CropState.IsGrowing;
        hasStartedGrowing = true;
        growthTimer = 0;
    }

    public void StopGrowing()
    {
        hasStartedGrowing = false;
    }

    public void OnCropMatured()
    {
        PlayCropMaturedSequence();
        currentState = CropState.ReadyToHarvest;
        OnCropMaturedEvent?.Invoke();
        StopGrowing();
    }

    private void UpdateGrowthProgress()
    {
        if(hasStartedGrowing)
        {
            if(growthTimer < Duration)
            {
                growthTimer += Time.deltaTime;
                if(growthTimer >= Duration)
                {
                    OnCropMatured();
                }
            }
        }
    }

    private void Update()
    {
        UpdateGrowthProgress();
    }

    public override void SetHighlight(bool flag)
    {
        cropRenderer.material = flag ? highlightMat : normalMat;
    }

    //private void OnMouseEnter()
    //{
    //    Debug.Log("OnMouseEnter");
    //    cropRenderer.material = highlightMat;

    //}

    //private void OnMouseExit()
    //{
    //    cropRenderer.material = normalMat;
    //}
}
