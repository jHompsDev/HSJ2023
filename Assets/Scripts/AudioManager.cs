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

    public void PlayBGM()
    {

    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
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
    public static IEnumerator SwitchBGM(string s)
    {
        AudioManager instance = GameManager.Instance.audioManager;

        AudioClip selectedAudio = instance.bgmSource.clip;

        foreach (AudioKey key in instance.sfxLibrary)
        {
            if (key.key == s) selectedAudio = key.clip;
        }

        if (instance.bgmSource.clip == selectedAudio) yield break;

        float t = 0f;
        float i = instance.bgmSource.volume;

        while (t < 1f)
        {
            t += Time.deltaTime;

            instance.bgmSource.volume = i - (i * (1 - t));
        }

        instance.bgmSource.Stop();

        instance.bgmSource.clip = selectedAudio;
        instance.bgmSource.volume = i;
        instance.bgmSource.Play();

        yield break;
    }
}
