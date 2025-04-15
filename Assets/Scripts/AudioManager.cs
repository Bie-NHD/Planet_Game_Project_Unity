using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AudioType
{
    Music,
    Sound,
}

public class AudioManager : MonoBehaviour
{
    [Header("-----------Audio Clip----------")]
    public AudioClip background;
    public AudioClip thow;
    public AudioClip merge;

    public AudioClip gameOver;

    private Dictionary<string, AudioWorker> audioDictionary;

    private void Awake()
    {
        audioDictionary = GetComponentsInChildren<AudioWorker>()
            .ToList()
            .ToDictionary(audioWorker => audioWorker.Key, audioWorker => audioWorker);
    }

    private void Start()
    {
        if (TryGetAudioSource(AudioType.Music, out AudioSource musicSource))
        {
            musicSource.clip = background;
            if (musicSource.enabled)
            {
                musicSource.Play();
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("AudioManager: AudioClip is null.");
            return;
        }

        if (audioDictionary.TryGetValue(GetKey(AudioType.Sound), out AudioWorker audioWorker))
        {
            audioWorker.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError($"AudioManager: AudioWorker not found for type {AudioType.Sound}.");
        }
    }

    public void PauseMusic()
    {
        if (audioDictionary.TryGetValue(GetKey(AudioType.Music), out AudioWorker audioWorker))
        {
            if (audioWorker.IsPlaying)
            {
                audioWorker.Pause();
            }
            else
            {
                Debug.LogWarning($"AudioManager: Music is not playing. Cannot pause.");
            }
        }
        else
        {
            Debug.LogError($"AudioManager: AudioWorker not found for type {AudioType.Music}.");
        }
    }

    public void UnPauseMusic()
    {
        if (audioDictionary.TryGetValue(GetKey(AudioType.Music), out AudioWorker audioWorker))
        {
            if (!audioWorker.IsPlaying)
            {
                audioWorker.UnPause();
            }
            else
            {
                Debug.LogWarning($"AudioManager: Music is already playing. Cannot unpause.");
            }
        }
        else
        {
            Debug.LogError($"AudioManager: AudioWorker not found for type {AudioType.Music}.");
        }
    }

    public string GetKey(AudioType type)
    {
        string name = Enum.GetName(typeof(AudioType), type) ?? string.Empty;
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError($"AudioManager: Invalid AudioType {type}.");
            return string.Empty;
        }
        return $"{name.ToLower()}_enabled";
    }

    public bool TryGetAudioSource(AudioType type, out AudioSource audioSource)
    {
        string key = GetKey(type);
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError($"AudioManager: Invalid AudioType {type}.");
            audioSource = null;
            return false;
        }

        audioSource = audioDictionary.TryGetValue(key, out AudioWorker audioWorker)
            ? audioWorker.AudioSource
            : null;
        if (audioSource == null)
        {
            Debug.LogError($"AudioManager: AudioSource not found for key {key}.");
            return false;
        }

        return true;
    }
}
