using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip testClip;
    public List<AudioKey> bgmLibrary;
    public List<AudioKey> sfxLibrary;

    public void AdjustBGMVol(float foo)
    {
        bgmSource.volume = foo;
        GameManager.Instance.uiManager.bgmVol.text = Mathf.Round(foo * 100).ToString() + "%";
    }

    public void AdjustSFXVol(float foo)
    {
        sfxSource.volume = foo;
        GameManager.Instance.uiManager.sfxVol.text = Mathf.Round(foo * 100).ToString() + "%";
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    [YarnCommand("playSFX")]
    public static void PlaySFX(string s)
    {
        AudioManager instance = GameManager.Instance.audioManager;

        foreach (AudioKey key in instance.sfxLibrary)
        {
            if (key.key == s)
            {
                instance.PlaySFX(key.clip);
                return;
            }
        }
        
        instance.PlaySFX(instance.testClip);
    }

    [YarnCommand("playBGM")]
    public static IEnumerator SwitchBGM(string s = "blank")
    {
        //Debug.Log("CALLING PLAYBGM");
        AudioManager instance = GameManager.Instance.audioManager;

        AudioClip selectedAudio = null;

        foreach (AudioKey key in instance.bgmLibrary)
        {
            if (key.key == s)
            {
                selectedAudio = key.clip;
                //Debug.Log("SELECTED KEY" + key.key);
            }

        }

        if (instance.bgmSource.clip == selectedAudio) yield break;

        float t = 0f;
        float i = instance.bgmSource.volume;

        if (instance.bgmSource.clip != null)
        {
            while (t < 1)
            {
                t += Time.deltaTime;

                instance.bgmSource.volume = i - (t * i);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }

        instance.bgmSource.Stop();
        instance.bgmSource.volume = i;
        instance.bgmSource.clip = selectedAudio;

        if (s != "blank")
        {
            //instance.bgmSource.clip = selectedAudio;
            instance.bgmSource.Play();
        }

        yield break;
    }
}
