using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip testClip;
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
        AudioManager instance = GameManager.Instance.audioManager;

        foreach (AudioKey key in instance.sfxSources)
        {
            if (key.key == s)
            {
                instance.PlaySFX(key.clip);
                return;
            }
        }
        
        instance.PlaySFX(instance.testClip);
    }
}
