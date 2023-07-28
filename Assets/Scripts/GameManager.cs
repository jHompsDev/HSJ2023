using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region VARIABLES
    public static GameManager Instance;

    [SerializeField]UIManager uIManager;

    bool debugMode;
    public bool DebugMode { get => debugMode; }

    GameState state;
    #endregion

    #region FUNCTIONS
    void Initialise()
    {
        // Singleton Setup
        if (Instance == null) Instance = this;
        else Destroy(this);

        if (debugMode) Debug.Log("Initialise!");

        if (uIManager == null) uIManager = FindObjectOfType<UIManager>();

        uIManager.Initialize();

        SwitchState(GameState.MAINMENU);
    }

    public void TrySwitchState(GameState newState)
    {
        switch (state)
        {
            case GameState.MAINMENU:
                if (newState == GameState.STORY) SwitchState(newState);
                else if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
            case GameState.STORY:
                if (newState == GameState.PAUSE) SwitchState(newState);
                else if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
            case GameState.PAUSE:
                if (newState == GameState.STORY || newState == GameState.PAUSE) SwitchState(newState);
                else if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
            default:
                if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
        }
    }

    void SwitchState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MAINMENU:
                uIManager.ToggleMainMenu(true);
                break;
            case GameState.STORY:
                uIManager.ToggleMainMenu(false);
                break;
            case GameState.PAUSE:
                uIManager.ToggleMainMenu(false);
                break;
            default: break;
        }

        state = newState;
        if (debugMode) Debug.Log("Case Switched to " + newState);
    }

    public void ToggleDebug(bool var)
    {
        debugMode = var;
    }
    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        ToggleDebug(true);

        Initialise();
    }
    #endregion
}

public enum GameState { MAINMENU, STORY, PAUSE}