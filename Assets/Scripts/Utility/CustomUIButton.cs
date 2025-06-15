using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum CustomTransitionType
{
    None,
    Color,
    SpriteSwap,
}

public class CustomUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
{
    AudioManager audioManager => AudioManager.Instance;
    [SerializeField] private bool interactableWhenPause;
    [SerializeField] Color highlightedColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1);
    [SerializeField] Color disabledColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.5f);
    public UnityEvent onClick;
    public Image image;
    public AudioClip[] sfxClick;
    public Vector3 highlightScale = Vector3.one * 0.9f;
    public Sprite highlightSprite;

    private Color baseColor = Color.white;
    private Sprite baseSprite;

    Vector3 onPointerDownScale = Vector3.one;
    public float animateDuration = 0.1f;
    public Ease animatedCurve = Ease.InBack;
    public bool enableVibration = false;
    private bool interactebleValue = true;
    public bool interactable 
    {
        get => interactebleValue;

        set
        {
            interactebleValue = value;
            if (value)
                EnableButton();
            else
                DisableButton();
        }
    }

    public float shrinkTime = 0.3f;

    Vector3 startScale;

    public CustomTransitionType transition = CustomTransitionType.None;

    public Sprite normalSprite;
    public Sprite disableSprite;
    public bool playSound = true;

    public bool isIgnoreAnimate;
    [SerializeField] private bool oneTimeClick = false;

    private void Awake() 
    {
        //SceneInstaller.Inject(this);
        if (image == null)
            image = GetComponent<Image>();
        if (image != null)
        {
            startScale = image.transform.localScale == Vector3.zero ? Vector3.one : image.transform.localScale;
        }
        onPointerDownScale = new Vector3(highlightScale.x * startScale.x, highlightScale.y * startScale.y, highlightScale.z * startScale.z);
        if (image != null && transition == CustomTransitionType.Color)
        {
            baseColor = image.color;
            baseSprite = image.sprite;
        }
    }

    private void Start()
    {
        
    }


    private void OnPauseUpdated(bool paused)
    {
        interactable = !paused;
    }

    private void OnEnable()
    {
        if (oneTimeClick) 
        {
            image.color = baseColor;
            interactable = true;
        }

        //Update interactable
        if (interactable)
            EnableButton();
        else
            DisableButton();
    }
    public void DisableButton() 
    {
        if (image != null && transition == CustomTransitionType.Color)
            image.color = disabledColor;

        if (image != null && transition == CustomTransitionType.SpriteSwap)
            image.sprite = disableSprite;
    }

    public void EnableButton()
    {
        if (image != null && transition == CustomTransitionType.Color)
            image.color = baseColor;

        if (image != null && transition == CustomTransitionType.SpriteSwap)
            image.sprite = normalSprite;
    }    

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;
        HighlightButton();

        if (isIgnoreAnimate) return;
        transform.DOScale(onPointerDownScale, animateDuration).SetEase(animatedCurve);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable || isIgnoreAnimate) return;
        transform.DOScale(startScale, animateDuration)
            .SetEase(animatedCurve)
            .OnComplete(() => ResetButton());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable || isIgnoreAnimate) return;
        transform.DOScale(startScale, animateDuration)
            .SetEase(animatedCurve)
            .OnComplete(() => ResetButton());
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (!interactable)
            return;
        onClick?.Invoke();
        if (oneTimeClick)
            interactable = false;

        if (audioManager != null)
        {
            if (sfxClick.Length <= 0)
                audioManager.PlaySFX("Click", 1f);
            else
                audioManager.PlaySFX(sfxClick.GetRandomElement(), 1f);
        }
    }

    public void HighlightButton() 
    {
        if (image != null) {
            switch (transition) {
                case CustomTransitionType.None:
                    break;
                case CustomTransitionType.Color:
                    image.color = highlightedColor;
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetButton()
    {
        if (image == null) return;

        switch (transition) {
            case CustomTransitionType.None:
                break;
            case CustomTransitionType.Color:
                image.color = baseColor;
                break;
            default:
                break;
        }
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void Shrink()
    {
        transform.DOScale(Vector3.zero, shrinkTime).SetEase(Ease.InBack);
        //LeanTween.scale(gameObject, Vector3.zero, shrinkTime).setEaseInBack().setIgnoreTimeScale(true);
    }
}