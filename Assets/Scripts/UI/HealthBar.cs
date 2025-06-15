using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image hpFill;
    [SerializeField] Image easeHpFill;
    [SerializeField] float easeDuration;

    private BreakableObject linkedObject;

    public bool IsAvailable => linkedObject == null;
    public void Bind(BreakableObject targetObject)
    {
        linkedObject = targetObject;
        linkedObject.OnHealthChangedEvent += OnHealthChanged;
        linkedObject.OnDeadEvent += OnDead;
        transform.position = targetObject.healthBarPos.position;
        canvasGroup.alpha = 0;
        hpFill.fillAmount = 1f;
        easeHpFill.fillAmount = 1f;
    }

    public void Unbind()
    {
        linkedObject.OnHealthChangedEvent -= OnHealthChanged;
        linkedObject.OnDeadEvent -= OnDead;
        linkedObject = null;
    }


    Sequence onHpChangedSequence;
    public void OnHealthChanged()
    {
        onHpChangedSequence?.Kill();
        onHpChangedSequence = DOTween.Sequence();
        onHpChangedSequence.Append(root.DOShakePosition(0.3f,15,35));
        onHpChangedSequence.Join(canvasGroup.DOFade(1f, 0.1f));
        onHpChangedSequence.Join(hpFill.DOFillAmount(linkedObject.HpPercentage, 0.1f).SetEase(Ease.OutQuad));
        onHpChangedSequence.Append(easeHpFill.DOFillAmount(linkedObject.HpPercentage, easeDuration).SetEase(Ease.OutQuad));
        onHpChangedSequence.AppendInterval(0.25f);
        onHpChangedSequence.Append(canvasGroup.DOFade(0f, 0.1f));

    }

    Sequence onDeadSequence;
    public void OnDead()
    {
        onDeadSequence?.Kill();
        onDeadSequence = DOTween.Sequence();
        onDeadSequence.Append(root.DOShakePosition(0.3f, 15, 35));
        onDeadSequence.Join(canvasGroup.DOFade(1f, 0.1f));
        onDeadSequence.Join(hpFill.DOFillAmount(linkedObject.HpPercentage, 0.1f).SetEase(Ease.OutQuad));
        onDeadSequence.Append(easeHpFill.DOFillAmount(linkedObject.HpPercentage, easeDuration).SetEase(Ease.OutQuad));
        onDeadSequence.Append(canvasGroup.DOFade(0f, 0.1f));
    }

}
