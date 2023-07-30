using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] GameObject LineDivider;

    [SerializeField] Image Background;
    [SerializeField] Image Foreground;
    [SerializeField] Image BGFadeOut;

    [SerializeField] public TMP_Text bgmVol, sfxVol;

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

    public void ToggleForeground(bool toggle)
    {
        Foreground.gameObject.SetActive(toggle);
    }

    [YarnCommand("actor")]
    public static IEnumerator ChangeActor(string actor = "blank")
    {
        UIManager ui = GameManager.Instance.uiManager;
        Actor nextActor = ui.actors[0];

        foreach(Actor a in ui.actors)
        {
            if (a.name == actor) nextActor = a;
        }

        float tMax = 0.33f;

        float t;

        if (ui.currentActor.name != "blank")
        {
            t = 0;

            while (t < tMax)
            {
                t += Time.deltaTime;

                ui.ActorRenderer.color = new Color(1f, 1f, 1f, 1 - (t / tMax));

                yield return null;
            }
        }

        ui.currentActor = nextActor;
        ui.ActorRenderer.sprite = ui.currentActor.sprite;
        yield return new WaitForSeconds(0.33f);

        if (ui.currentActor.name != "blank")
        {
            t = 0f;

            while (t < tMax)
            {
                t += Time.deltaTime;

                ui.ActorRenderer.color = new Color(1f, 1f, 1f, t / tMax);

                yield return null;
            }
        }

        yield break;
    }

    //[YarnCommand("actor")]
    //public static void ChangeActor(string actor = "blank")
    //{
    //    UIManager ui = GameManager.Instance.uiManager;

    //    foreach (Actor a in ui.actors)
    //    {
    //        if (a.name == actor) ui.currentActor = a;
    //    }

    //    GameManager.Instance.uiManager.ToggleLineDivider(actor != "blank");

    //    ui.ActorRenderer.sprite = ui.currentActor.sprite;
    //}

    public void ToggleLineDivider(bool foo)
    {
        LineDivider.SetActive(foo);
    }

    public void PlayActorBlip()
    {
        Debug.Log("Blip");
        if (currentActor.name != "blank") GameManager.Instance.audioManager.PlaySFX(currentActor.sound);
    }

    [YarnCommand("bgFade")]
    public static IEnumerator FadeBackground(bool foo)
    {
        UIManager instance = GameManager.Instance.uiManager;

        float alpha = foo ? 1.0f : 0.0f;
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime;
            instance.BGFadeOut.color = new Color(0f, 0f, 0f, foo ? 0 + t : 1 - t);
            yield return null;
        }

        yield break;
    }

    public static IEnumerator FadeForeground(bool foo)
    {
        UIManager instance = GameManager.Instance.uiManager;

        float alpha = foo ? 1.0f : 0.0f;
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime;
            instance.Foreground.color = new Color(0f, 0f, 0f, foo ? 0 + t : 1 - t);
            yield return null;
        }

        yield break;
    }

    #region BUTTONS()
    public void StartButton()
    {
        AudioManager.PlaySFX("uiButtPress");
        GameManager.Instance.TrySwitchState(GameState.STORY);
    }

    public void OptionsButton()
    {
        AudioManager.PlaySFX("uiButtPress");

        ToggleMainMenu(false);
        TogglePauseUI(false);
        ToggleSettingsMenu(true);
    }

    public void BackButton()
    {
        AudioManager.PlaySFX("uiButtPress");

        if (GameManager.Instance.state == GameState.MAINMENU) ToggleMainMenu(true);
        else TogglePauseUI(true);
        ToggleSettingsMenu(false);
    }

    public void PauseButton()
    {
        AudioManager.PlaySFX("uiButtPress");

        GameManager.Instance.TrySwitchState(GameState.PAUSE);
    }

    public void ResumeButton()
    {
        AudioManager.PlaySFX("uiButtPress");

        GameManager.Instance.TrySwitchState();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    #endregion
}
