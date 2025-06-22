using UnityEngine;

public class ThankYouView : UIView
{
    UserData userData => GameManager.Instance.userData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userData.OnExpeditionUnlock += OnUnlockExpedition;
    }

    private void OnDestroy()
    {
        userData.OnExpeditionUnlock -= OnUnlockExpedition;
    }

    private void OnUnlockExpedition() => Show(true);
}
