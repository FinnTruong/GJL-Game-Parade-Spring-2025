using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialMenuItem : MonoBehaviour
{
    public string menuTitle;
    [SerializeField] Image icon;
    [SerializeField] Color normalColor;
    [SerializeField] Color highlightColor;
    [SerializeField] AudioClip selectItemSFX;
    [ShowIf(nameof(hasSFX))]
    [SerializeField] float sfxVolume = 0.7f;

    bool hasSFX => selectItemSFX != null;

    public System.Action OnHighlightItem;
    public System.Action OnSelectItem;

    public UnityEvent onSelectItemEvent;

    Vector3 startScale;

    private void Awake()
    {
        startScale = icon.transform.localScale;
    }

    public void SetIconRot()
    {
        icon.transform.rotation = Quaternion.identity;
    }

    public void DeselectItem()
    {
        icon.DOKill();
        icon.transform.DOScale(startScale, 0.1f);
        //icon.DOColor(normalColor, 0.1f);
    }

    public void HighlightItem()
    {
        AudioManager.Instance.PlaySFX("Click", 0.7f);
        icon.DOKill();
        icon.transform.DOScale(startScale * 1.25f, 0.1f);
        //icon.DOColor(highlightColor, 0.1f);
        OnHighlightItem?.Invoke();
    }   
    
    public virtual void Select()
    {
        Debug.Log("Select Item");
        OnSelectItem?.Invoke();
        onSelectItemEvent?.Invoke();
        if (selectItemSFX != null)
            AudioManager.Instance.PlaySFX(selectItemSFX, sfxVolume);
    }

}
