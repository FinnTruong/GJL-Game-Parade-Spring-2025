using DG.Tweening;
using Mono.Cecil;
using System.Resources;
using TMPro;
using UnityEngine;



public class BindTextToResources : MonoBehaviour
{
    [SerializeField] ResourceType resourceType;
    [SerializeField] Color increaseColor;
    [SerializeField] Color decreaseColor;
    [SerializeField] float minDifference;

    UserData userData => GameManager.Instance.userData;

    TMP_Text resourceText;
    float oldValue;
    Color startColor;
    private void Awake()
    {
        resourceText = GetComponent<TMP_Text>();
        startColor = resourceText.color;
    }

    void Start()
    {
        userData.OnGoldLeafChanged += OnResourceChanged;
    }

    private void OnDestroy()
    {
        userData.OnGoldLeafChanged -= OnResourceChanged;
    }



    private void OnResourceChanged()
    {
        PlayTextEffect(Mathf.Round(GetCurrentResource()));
    }
    private float GetCurrentResource()
    {
        switch (resourceType)
        {
            case ResourceType.GoldLeaf:
                return userData.GoldLeaf;            
            default:
                return 0;
        }
    }

    Tweener animationTween;
    private void PlayTextEffect(float newValue)
    {
        if (Mathf.Abs(oldValue - newValue) < minDifference)
        {
            resourceText.SetText(Utility.MoneyToString(newValue));
            oldValue = newValue;
            return;
        }

        Color textEffectColor = oldValue < newValue ? increaseColor : decreaseColor;
        animationTween?.Complete();
        animationTween = DOVirtual.Float(oldValue, newValue, 0.2f, v =>
        {
            v = Mathf.Round(v);
            float lerpValue = Mathf.InverseLerp(oldValue, newValue, v);
            resourceText.color = Color.Lerp(startColor, textEffectColor, lerpValue);
            resourceText.SetText(Utility.MoneyToString(v));
        }).OnComplete(() =>
        {
            resourceText.DOColor(startColor, 0.15f).SetUpdate(true);
            oldValue = newValue;
        }).SetUpdate(true);
    }
}
