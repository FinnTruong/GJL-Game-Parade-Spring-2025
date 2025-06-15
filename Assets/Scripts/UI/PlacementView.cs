using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class PlacementView : UIView
{
    [SerializeField] Tilemap grid;
    GameManager gameManager => GameManager.Instance;

    [SerializeField] AudioClip openSFX, closeSFX;

    protected override void OnShow()
    {
        base.OnShow();
        viewManager.HideAllView();
        GameManager.Instance.CurrentState = GameState.PlacementState;
        grid.gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(openSFX, 0.15f);
        //DOVirtual.Float(0, 1, 0.25f, x => grid.color = new Color(1, 1, 1, x));
    }


    protected override void OnHide()
    {
        base.OnHide();
        viewManager.hudView.Show();
        GameManager.Instance.CurrentState = GameState.None;
        AudioManager.Instance.PlaySFX(closeSFX, 0.5f);
        //DOVirtual.Float(1, 0, 0.25f, x => grid.color = new Color(1, 1, 1, x))
        //    .OnComplete(()=>grid.gameObject.SetActive(false));
    }
}
