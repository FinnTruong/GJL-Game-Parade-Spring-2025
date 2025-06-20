using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    UserData userData => GameManager.Instance.userData;
    CardType id;

    private void OnMouseDown()
    {
        userData.AddCardToHand(id);
        Destroy(gameObject);
    }
}
