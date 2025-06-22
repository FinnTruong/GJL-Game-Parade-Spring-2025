using System.Collections;
using UnityEngine;

public class StartView : UIView
{
    UserData userData => GameManager.Instance.userData;

    [SerializeField] AudioClip unlockSFX;

    protected override void OnHide()
    {
        base.OnHide();
        AudioManager.Instance.PlaySFX(unlockSFX);
        StartCoroutine(SpawnStartingHand());
    }

    IEnumerator SpawnStartingHand()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 5; i++)
        {
            userData.AddCardToHand(CardType.Lemon);
            AudioManager.Instance.PlaySFX("DrawCard");
            yield return new WaitForSeconds(0.2f);
        }
    }
}
