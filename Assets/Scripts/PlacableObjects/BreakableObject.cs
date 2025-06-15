using DG.Tweening;
using UnityEngine;

public class BreakableObject : PlaceableObject
{
    public System.Action OnHealthChangedEvent;
    public System.Action OnDeadEvent;

    public Material normalMat;
    public Material highlightMat;

    public int maxHP;
    public int currentHP;

    public ParticleSystem sparkVFX;
    public ParticleSystem smokePuffVFX;

    public Transform healthBarPos;

    public float HpPercentage => Mathf.Clamp01((float)currentHP / maxHP);

    private bool isDead;

    public void Start()
    {
        
    }

    public override void OnConfirmPlacement()
    {
        base.OnConfirmPlacement();
        HealthBarManager.Instance.BindToHealthBar(this);

    }

    public override void OnClicked()
    {
        base.OnClicked();
        OnHit();
    }

    public override void SetHighlight(bool flag)
    {
        sr.material = flag ? highlightMat : normalMat;
    }

    public void OnHit()
    {
        if (currentHP <= 0)
            return;
        if (sparkVFX != null)
            sparkVFX.Play();
        currentHP -= 1;
        if (currentHP <= 0)
        {
            OnDead();
        }
        else
        {
            OnHealthChangedEvent?.Invoke();
        }    
    }

    Sequence onDeadSequence;
    public void OnDead()
    {
        OnDeadEvent?.Invoke();
        onDeadSequence?.Complete();
        onDeadSequence = DOTween.Sequence();
        onDeadSequence.AppendInterval(0.15f);
        onDeadSequence.Append(sr.DOFade(0, 0.25f).SetEase(Ease.OutQuad));
        onDeadSequence.AppendCallback(() =>
        {
            RemoveObject();
        });

    }
}
