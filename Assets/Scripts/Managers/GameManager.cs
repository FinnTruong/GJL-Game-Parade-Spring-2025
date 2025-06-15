using UnityEngine;

public enum GameState
{
    None,
    PlacementState,
    ToolState,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UserData userData = new();

    private GameState currentState;
    public GameState CurrentState
    { 
        get => currentState;
        set
        {
            currentState = value;
            OnGameStateChanged?.Invoke();
        }
    }

    public System.Action OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
