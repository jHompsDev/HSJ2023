using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]GameObject MainMenuObject; 

    public void Initialize()
    {
        Debug.Log("UI Initialized");

        if (MainMenuObject == null) GameObject.Find("Main Menu");
        if (GameManager.Instance.DebugMode) Debug.Log("Main Menu found under " + MainMenuObject.name);
    }

    public void ToggleMainMenu(bool toggle)
    {
        if (GameManager.Instance.DebugMode) Debug.Log("Main Menu Toggled " + (toggle ? "On" : "Off"));
    }

    #region BUTTONS()
    public void StartButton()
    {
        GameManager.Instance.TrySwitchState(GameState.STORY);
    }

    public void OptionsButton()
    {

    }

    public void QuitButton()
    {
        Application.Quit();
    }
    #endregion
}
