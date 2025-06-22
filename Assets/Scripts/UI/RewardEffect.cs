using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RewardEffect : MonoBehaviour
{
    private Vector3 startPos;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text rewardAmountText;

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    [ContextMenu("Play")]
    public void PlayEffect(Vector2 spawnPos, ResourceType reward, int amount)
    {
        transform.position = spawnPos;
        startPos = transform.localPosition;
        var spriteString = $"<sprite={(int)reward}>";
        rewardAmountText.SetText($"+{amount}{spriteString}");
        gameObject.SetActive(true);
        PlayMoveSequence();
        PlayFadeSequence();
    }

    Sequence moveSequence;
    private void PlayMoveSequence()
    {
        transform.localPosition = startPos;
        moveSequence?.Complete();
        moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOLocalMoveY(startPos.y + 100f, 1f));
        moveSequence.AppendCallback(() => Destroy(gameObject));
    }

    Sequence fadeSequence;
    private void PlayFadeSequence()
    {
        fadeSequence?.Complete();
        canvasGroup.alpha = 0.25f;
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(canvasGroup.DOFade(1, 0.2f).SetEase(Ease.InQuad));
        fadeSequence.AppendInterval(0.5f);
        fadeSequence.Append(canvasGroup.DOFade(0, 0.3f));

    }
}
