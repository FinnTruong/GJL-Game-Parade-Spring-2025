using IngameDebugConsole;
using UnityEngine;

public class DebugCheat : MonoBehaviour
{
    UserData userData => GameManager.Instance.userData;
    void Start()
    {
#if !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        DebugLogConsole.AddCommand<int>("xp", "Add XP", AddXp);


    }

    private void AddXp(int xp)
    {
        userData.Xp += xp;
    }

}
