using UnityEngine;

public class Toolbar : MonoBehaviour
{
    [SerializeField] ToolSlot[] tools;

    public System.Action OnChangedTool;
    void Start()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].OnSelectTool += OnSelectTool;
        }
    }

    private void OnSelectTool()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].DeselectTool();
        }
        OnChangedTool?.Invoke();

    }

    public void UpdateSlot()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].UpdateUI();
        }
    }
}
