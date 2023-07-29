using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    #region VARIABLES
    public static GameManager Instance;

    [SerializeField] public UIManager uiManager;
    [SerializeField] public AudioManager audioManager;
    [SerializeField] public ConstellationManager conManager;

    bool debugMode;
    public bool DebugMode { get => debugMode; }

    public GameState state, lastState;

    public static bool test;
    #endregion

    #region FUNCTIONS
    void Initialise()
    {
        // Singleton Setup
        if (Instance == null) Instance = this;
        else Destroy(this);

        if (debugMode) Debug.Log("Initialise!");

        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        if (audioManager == null) audioManager = FindObjectOfType<AudioManager>();

        uiManager.Initialize();

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
                if (newState == GameState.PAUSE || newState == GameState.STARS) SwitchState(newState);
                else if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
            case GameState.PAUSE:
                if (newState == GameState.STORY || newState == GameState.STARS) SwitchState(newState);
                else if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
            case GameState.STARS:
                if (newState == GameState.STORY || newState == GameState.PAUSE) SwitchState(newState);
                else if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
            default:
                if (debugMode) Debug.LogWarning("Game State can't be toggled from " + state + " to " + newState);
                break;
        }
    }

    public void TrySwitchState()
    {
        TrySwitchState(lastState);
    }

    void SwitchState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MAINMENU:
                uiManager.ToggleMainMenu(true);
                uiManager.ToggleDialogueUI(false);
                uiManager.TogglePauseUI(false);
                conManager.gameObject.SetActive(false);
                break;
            case GameState.STORY:
                uiManager.ToggleMainMenu(false);
                uiManager.ToggleDialogueUI(true);
                uiManager.TogglePauseUI(false);
                conManager.gameObject.SetActive(false);
                break;
            case GameState.PAUSE:
                uiManager.ToggleMainMenu(false);
                uiManager.ToggleDialogueUI(false);
                uiManager.TogglePauseUI(true);
                conManager.gameObject.SetActive(false);
                break;
            case GameState.STARS:
                uiManager.ToggleMainMenu(false);
                uiManager.ToggleDialogueUI(false);
                uiManager.TogglePauseUI(false);
                conManager.gameObject.SetActive(true);
                break;
            default: break;
        }

        lastState = state;
        state = newState;
        if (debugMode) Debug.Log("Case Switched to " + newState);
    }

    public void ToggleDebug(bool var)
    {
        debugMode = var;
    }
    #endregion

    #region COROUTINES / YARN COMMANDS
    [YarnCommand("constellation")]
    public static IEnumerator WaitForConstellation(int index)
    {
        Instance.TrySwitchState(GameState.STARS);

        yield return Instance.StartCoroutine(IConstellation(index));

        yield return new WaitForSeconds(5);

        Instance.TrySwitchState(GameState.STORY);
        
        yield return null;
    }

    public static IEnumerator IConstellation(int index)
    {
        Instance.conManager.Initialise(index - 1);

        while (Instance.conManager.currentConstellation.isComplete != true)
        {
            yield return null;
        }

        yield break;
    }

    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        ToggleDebug(true);

        Initialise();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == GameState.STORY || state == GameState.STARS)
            {
                if (state == GameState.STARS) conManager.firstText.gameObject.SetActive(false);
                TrySwitchState(GameState.PAUSE);
            }
            else if (state == GameState.PAUSE) TrySwitchState();
        }
    }
    #endregion
}

public enum GameState { MAINMENU, STORY, PAUSE, STARS}