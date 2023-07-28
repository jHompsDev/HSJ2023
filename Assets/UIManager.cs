using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenuObject;
    [SerializeField] GameObject PauseMenuObject;
    [SerializeField] GameObject SettingsMenuObject;

    [SerializeField] GameObject LineView;
    [SerializeField] GameObject OptionsListView;
    [SerializeField] DialogueRunner DialogueRunner;

    public void Initialize()
    {
        Debug.Log("UI Initialized");

        if (MainMenuObject == null) GameObject.Find("Main Menu");
        if (GameManager.Instance.DebugMode) Debug.Log("Main Menu found under " + MainMenuObject.name);
    }

    public void ToggleMainMenu(bool toggle)
    {
        MainMenuObject.SetActive(toggle);

        if (GameManager.Instance.DebugMode) Debug.Log("Main Menu Toggled " + (toggle ? "On" : "Off"));
    }

    public void ToggleSettingsMenu(bool toggle)
    {
        SettingsMenuObject.SetActive(toggle);

        if (GameManager.Instance.DebugMode) Debug.Log("Settings Menu Toggled " + (toggle ? "On" : "Off"));
    }

    public void ToggleDialogueUI(bool toggle)
    {
        LineView.SetActive(toggle);
        OptionsListView.SetActive(toggle);

        DialogueRunner.enabled = toggle;
    }

    #region BUTTONS()
    public void StartButton()
    {
        GameManager.Instance.TrySwitchState(GameState.STORY);
    }

    public void OptionsButton()
    {
        ToggleMainMenu(false);
        ToggleSettingsMenu(true);
    }

    public void BackButton()
    {
        ToggleMainMenu(true);
        ToggleSettingsMenu(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    #endregion
}
