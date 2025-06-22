using DG.Tweening;
using Febucci.UI;
using RTGJ;
using TMPro;
using UIAnimatorCore;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] RectTransform popup;
    [SerializeField] UIAnimator uiAnimator;
    [SerializeField] TextAnimator_TMP text;

    [SerializeField] Image notificationIcon;
    [SerializeField] Vector2 startSizeDelta;
    [SerializeField] Vector2 endSizeDelta;

    [SerializeField] AudioClip popupSFX;

    UserData userData => GameManager.Instance.userData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userData.OnLevelChanged += ShowPopup;
    }

    private void OnDestroy()
    {
        userData.OnLevelChanged -= ShowPopup;
    }

    Sequence triggerPopupSequence;
    public void ShowPopup()
    {
        notificationIcon.transform.rotation = Quaternion.identity;
        notificationIcon.gameObject.SetActive(true);
        notificationIcon.DOFade(1f, 0f);
        text.gameObject.SetActive(false);

        triggerPopupSequence?.Complete();
        triggerPopupSequence = DOTween.Sequence();
        triggerPopupSequence.AppendCallback(() =>
        {
            root.gameObject.SetActive(true);
            popup.sizeDelta = startSizeDelta;
            popup.gameObject.SetActive(true);
            if (popupSFX != null)
                AudioManager.Instance.PlaySFX(popupSFX);
        });
        triggerPopupSequence.AppendInterval(1f);
        triggerPopupSequence.Append(popup.DOSizeDelta(endSizeDelta, 0.5f).SetEase(Ease.OutQuad));
        triggerPopupSequence.Join(notificationIcon.transform.DORotate(new Vector3(0, 0, 180f), 1f, RotateMode.LocalAxisAdd));
        triggerPopupSequence.Join(notificationIcon.DOFade(0f, 0.5f));
        triggerPopupSequence.JoinCallback(() =>
        {           
            text.gameObject.SetActive(true);

        }).SetDelay(0.35f);
        triggerPopupSequence.AppendCallback(() => notificationIcon.gameObject.SetActive(false));
        triggerPopupSequence.AppendInterval(1f);
        triggerPopupSequence.AppendCallback(() => uiAnimator.PlayAnimation(AnimSetupType.Outro, () =>
        {
            text.gameObject.SetActive(false);
            popup.gameObject.SetActive(false);
        }));
    }
}
