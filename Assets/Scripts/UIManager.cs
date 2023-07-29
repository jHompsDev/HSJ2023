using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
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

    [SerializeField] Image Background;
    [SerializeField] Image BGOverlay;

    [SerializeField] Image ActorRenderer;

    [SerializeField] List<Actor> actors;
    Actor currentActor;

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

    public void TogglePauseUI(bool toggle)
    {
        PauseMenuObject.SetActive(toggle);

        if (GameManager.Instance.DebugMode) Debug.Log("Pause Menu Toggled " + (toggle ? "On" : "Off"));
    }

    [YarnCommand("actor")]
    public static void ChangeActor(string actor)
    {
        UIManager ui = GameManager.Instance.uiManager;

        foreach (Actor a in ui.actors)
        {
            if (a.name == actor) ui.currentActor = a;
        }

        if (actor == null) return;

        ui.ActorRenderer.sprite = ui.currentActor.sprite;
    }

    #region BUTTONS()
    public void StartButton()
    {
        GameManager.Instance.TrySwitchState(GameState.STORY);
    }

    public void OptionsButton()
    {
        ToggleMainMenu(false);
        TogglePauseUI(false);
        ToggleSettingsMenu(true);
    }

    public void BackButton()
    {
        if (GameManager.Instance.state == GameState.MAINMENU) ToggleMainMenu(true);
        else TogglePauseUI(true);
        ToggleSettingsMenu(false);
    }

    public void PauseButton()
    {
        GameManager.Instance.TrySwitchState(GameState.PAUSE);
    }

    public void ResumeButton()
    {
        GameManager.Instance.TrySwitchState();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    #endregion
}
