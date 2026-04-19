using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public enum SoundType
{
    SFX,
    BGM
}

[System.Serializable]
public class AudioClips
{
    [SerializeField]
    public List<AudioClass> clips;
}


[System.Serializable]
public class AudioClass
{
    public string name;
    public AudioClip clip;
    public SoundType type;
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Mixer")]
    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    [Header("Source")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClips audioClips;
    [Header("Main Theme")]
    public AudioClip bgm;

    [Header("Current Volumes")]
    public float bgmVolume;
    public float sfxVolume;
    public float dempAmount;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bgmSource.clip = bgm;
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = audioClips.clips.Find(soundclip =>  soundclip.name == soundName).clip;
    }

    public void playSFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }

    public void SetDempAmount(float amount)
    {
        dempAmount = amount;
        bgmMixer.SetFloat("BGMLowPass", amount);
    }
}
