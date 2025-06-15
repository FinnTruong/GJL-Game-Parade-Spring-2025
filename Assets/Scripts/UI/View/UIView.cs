using UnityEngine;
using Sirenix.OdinInspector;
using UIAnimatorCore;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    Canvas canvas;
    GraphicRaycaster graphicRaycaster;
    UIAnimator uiAnimator;

    protected ViewManager viewManager =>ViewManager.Instance;
    protected UserData userData => GameManager.Instance.userData;
    protected ConfigManager configManager => ConfigManager.Instance;

    public bool IsOpen => canvas != null && canvas.enabled;
    private bool hasInit;

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        uiAnimator = GetComponentInChildren<UIAnimator>();
    }

    private void Init()
    {
        if (hasInit)
            return;
        hasInit = true;
        OnInit();
    }

    protected virtual void OnInit() { }

    public virtual void Show(bool playAnimation = true)
    {
        if (IsOpen)
            return;

        Init();
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        canvas.enabled = true;
        graphicRaycaster.enabled = true;
        OnShow();
        if (uiAnimator != null && playAnimation)
        {
            uiAnimator.PlayAnimation(AnimSetupType.Intro);
        }
    }

    protected virtual void OnShow() { }
    public virtual void Hide()
    {
        if (!IsOpen)
            return;

        OnHide();
        if (uiAnimator != null)
        {
            uiAnimator.PlayAnimation(AnimSetupType.Outro, () =>
            {
                OnCompleteOutro();
                canvas.enabled = false;
                graphicRaycaster.enabled = false;
            });
        }
        else
        {
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }
    }

    protected virtual void OnHide() { }

    protected virtual void OnCompleteOutro() { }



}
