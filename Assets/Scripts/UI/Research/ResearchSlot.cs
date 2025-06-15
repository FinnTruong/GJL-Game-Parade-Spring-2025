using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchSlot : MonoBehaviour
{
    [SerializeField] ResearchType researchType;
    [SerializeField] GameObject availableRoot;
    [SerializeField] GameObject unavailableRoot;
    [SerializeField] TMP_Text priceText;
    [SerializeField] CustomUIButton unlockButton;
    [SerializeField] GameObject checkmark;

    UserData userData => GameManager.Instance.userData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userData.OnResearchAdded += UpdateUI;
    }

    private void OnDestroy()
    {
        userData.OnResearchAdded -= UpdateUI;
    }


    void UpdateUI()
    {

    }
    
}
