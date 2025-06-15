using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CropProgressWheel : MonoBehaviour
{
    [SerializeField] Transform progressWheel;
    [SerializeField] Image fill;
    [SerializeField] ParticleSystem completeVFX;

    private Crop linkedCrop;

    Sequence onCompleteSequence;
    Vector3 startScale;

    public bool IsAvailable => linkedCrop == null;
    private void Awake()
    {
        startScale = progressWheel.localScale;
    }

    public void Bind(Crop crop)
    {
        linkedCrop = crop;
        linkedCrop.OnCropMaturedEvent += OnComplete;
        transform.position = crop.progressWheelPos.position;
        gameObject.SetActive(true);
        progressWheel.localScale = startScale;
    }

    public void Unbind()
    {
        gameObject.SetActive(false);
        linkedCrop.OnCropMaturedEvent -= OnComplete;
        linkedCrop = null;
    }

    private void Update()
    {
        if (linkedCrop != null)
            fill.fillAmount = linkedCrop.GrowthProgress;
    }




    private void OnDestroy()
    {

    }

    public void UpdatePanel()
    {

    }

    public void OnComplete()
    {
        //onCompleteWateringSequence?.Complete();
        onCompleteSequence = DOTween.Sequence();
        onCompleteSequence.AppendCallback(() => completeVFX.Play());
        onCompleteSequence.Append(progressWheel.DOScale(startScale * 1.5f, 0.25f));
        onCompleteSequence.Append(progressWheel.DOScale(0, 0.2f).SetEase(Ease.OutQuad));
        onCompleteSequence.AppendCallback(() => Unbind());

    }
}
