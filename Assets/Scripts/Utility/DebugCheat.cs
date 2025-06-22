using IngameDebugConsole;
using System;
using UnityEngine;

public class DebugCheat : MonoBehaviour
{
    UserData userData => GameManager.Instance.userData;
    void Start()
    {
#if !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        DebugLogConsole.AddCommand<int>("leaf", "Add Leaf", AddLeaf);
        DebugLogConsole.AddCommand<int>("xp", "Add XP", AddXp);
        DebugLogConsole.AddCommand("unlockallresearch", "Unlock All Research", UnlockAllResearch);

    }

    private void AddLeaf(int value)
    {
        userData.GoldLeaf += value;
    }

    private void AddXp(int xp)
    {
        userData.Xp += xp;
    }

    private void UnlockAllResearch()
    {
        foreach (ResearchType research in Enum.GetValues(typeof(ResearchType)))
        {
            userData.AddResearch(research);
        }
    }

}
