using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTGJ
{
    public class HoldUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject highlight;
        [SerializeField] Image button;
        [SerializeField] Image fill;

        public System.Action OnPointerEnterEvent;
        public System.Action OnPointerExitEvent;
        public System.Action OnPointerDownEvent;
        public System.Action OnPointerUpEvent;

        public UnityEvent onClicked;

        public Vector3 highlightScale = Vector3.one * 0.9f;

        Vector3 onPointerDownScale = Vector3.one;
        public float animateDuration = 0.1f;
        public Ease animatedCurve = Ease.InBack;

        public float holdDuration = 0.3f;
        public float timer;

        public bool isClicking;
        public bool hasClicked;

        public AudioClip confirmSFX;
        private float currentProgress => Mathf.Clamp01(timer / holdDuration);

        private bool interactebleValue = true;
        public bool interactable
        {
            get => interactebleValue;
            set => interactebleValue = value;
        }

        Vector3 startScale;


        [Range(0, 1)]
        public float alphaHitTestMinimumThreshold = 0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (button == null)
                button = GetComponent<Image>();
            if (button != null)
                startScale = button.transform.localScale == Vector3.zero ? Vector3.one : button.transform.localScale;

            onPointerDownScale = new Vector3(highlightScale.x * startScale.x, highlightScale.y * startScale.y, highlightScale.z * startScale.z);

            if (button.mainTexture.isReadable)
                button.alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;
        }

        // Update is called once per frame
        void Update()
        {
            if (!interactable)
                return;
            if (isClicking)
            {
                timer += Time.unscaledDeltaTime;
                fill.fillAmount = currentProgress;
                if (timer > holdDuration)
                {
                    transform.DOKill();
                    transform.DOScale(startScale * 1.05f, 0.1f)
                        .OnComplete(()=>transform.DOScale(startScale,0.1f))
                        .SetUpdate(true);
                    fill.fillAmount = 0;
                    if (highlight != null)
                        highlight.SetActive(false);
                    isClicking = false;
                    if (confirmSFX == null)
                        AudioManager.Instance.PlaySFX("Confirm", 0.5f);
                    else
                        AudioManager.Instance.PlaySFX(confirmSFX);
                    onClicked?.Invoke();
                }
            }
        }


        public void Init()
        {
            hasClicked = false;
            ResetProgress();
        }

        public void ResetProgress()
        {
            //if (highlight != null)
            //    highlight.SetActive(false);
            timer = 0;
            fill.fillAmount = 0;
            isClicking = false;
            hasClicked = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) 
                return;
            if (hasClicked)
                return;
            AudioManager.Instance.PlaySFX("Click");
            transform.DOKill();
            transform.DOScale(onPointerDownScale, animateDuration).SetEase(animatedCurve).SetUpdate(true);
            OnPointerDownEvent?.Invoke();
            isClicking = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable)
                return;
            if (hasClicked)
                return;
            isClicking = false;
            ResetProgress();
            transform.DOKill();
            transform.DOScale(startScale, animateDuration).SetEase(animatedCurve).SetUpdate(true);
            OnPointerUpEvent?.Invoke();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hasClicked)
                return;
            if (highlight != null)
                highlight.SetActive(true);
            AudioManager.Instance.PlaySFX("Hover");
            OnPointerEnterEvent?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (hasClicked)
                return;
            OnPointerExitEvent?.Invoke();
            if (highlight != null)
                highlight.SetActive(false);

            isClicking = false;
            ResetProgress();
        }


    }
}
