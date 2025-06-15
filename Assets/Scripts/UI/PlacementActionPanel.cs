using UIAnimatorCore;
using UnityEngine;

public class PlacementActionPanel : MonoBehaviour
{
    [SerializeField] UIAnimator uiAnimator;

    public void Show()
    {
        if (uiAnimator.CurrentAnimType == AnimSetupType.Intro)
            return;
        gameObject.SetActive(true);
        uiAnimator.PlayAnimation(AnimSetupType.Intro);
    }

    public void Hide()
    {
        if (uiAnimator.CurrentAnimType == AnimSetupType.Outro)
            return;
        uiAnimator.PlayAnimation(AnimSetupType.Outro,()=>
        {
            gameObject.SetActive(false);
        });
    }
}
