using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class SoundManager
{
    public enum Sound
    {
        VictoryTheme,
        OnButtonClick,
        OnDeath,
        CountdownNumber,
        CountdownFight
    }

    public static void PlaySound(Sound sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound), volumeScale: 0.5f);
        
        if(!audioSource.isPlaying)
        {
            // Want to destroy to clean up game sounds... no such function in static void?
            gameObject.SetActive(false);
            // Can onl ydo this for now
        }
    }

    public static GameObject PlaySoundAndCleanUp(Sound sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound), volumeScale: 0.5f);

        return gameObject;
    }

    public static void PlayTheme(Sound sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        gameObject.tag = "Music";
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound), volumeScale: 0.1f);
    }
    
    public static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}