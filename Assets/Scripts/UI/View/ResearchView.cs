using UnityEngine;
using UnityEngine.UI;

public class ResearchView : UIView
{
    [SerializeField] ScrollRect scroll;

    protected override void OnShow()
    {
        base.OnShow();
        scroll.enabled = true;
    }

    protected override void OnHide()
    {
        base.OnHide();
        scroll.enabled = false;
    }
}
