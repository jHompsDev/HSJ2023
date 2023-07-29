using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public List<AudioKey> sfxSources;

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
        foreach (AudioKey key in GameManager.Instance.audioManager.sfxSources)
        {
            if (key.key == s) GameManager.Instance.audioManager.PlaySFX(key.clip);
        }
    }
}
