using System;
using UnityEngine;

public enum AudioType
{
    Music,
    Sound,
}

public class AudioManager : MonoBehaviour
{
    [Header("-----------Audio Sources----------")]
    [SerializeField]
    AudioSource _musicSource;

    [SerializeField]
    AudioSource _soundEffectSource;

        [SerializeField]
    private PlayerPrefsManager _playerPrefsManager;

    [Header("-----------Audio Clip----------")]
    public AudioClip background;
    public AudioClip thow;
    public AudioClip merge;

    [SerializeField]
    private PlayerPrefsManager _playerPrefsManager;

    private void Awake()
    {
        SetUpAudioSource();

        if (_playerPrefsManager == null)
        {
            Debug.LogError(
                "AudioManager: PlayerPrefsManager not found. AudioManager will not function properly."
            );
        }
    }

private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource.enabled == false)
        {
            Debug.Log("AudioManager: Sound source is disabled. Cannot play audio clip.");
            return;
        }
        _soundEffectSource.PlayOneShot(clip);
    }

    public void ToggleAudio(AudioType type, bool isOn)
    {
        switch (type)
        {
            case AudioType.Music:
                ToggleMusic(isOn);
                _playerPrefsManager.TogglePlayerPref(PlayerPrefsKeys.MusicPref, isOn);
                break;
            case AudioType.Sound:
                _soundEffectSource.enabled = !_soundEffectSource.enabled;
                _playerPrefsManager.TogglePlayerPref(PlayerPrefsKeys.SoundPref, isOn);
                break;
        }
    }

    public void ToggleMusic(bool isOn)
    {
        switch (isOn)
        {
            case true:

                if (_musicSource.enabled == false)
                {
                    _musicSource.enabled = true;
                }

                if (_musicSource.isPlaying == false)
                {
                    _musicSource.Play();
                    break;
                }
                else
                {
                    _musicSource.UnPause();
                    break;
                }
            case false:
                _musicSource.Pause();
                break;
        }
    }

    private void SetUpAudioSource()
    {
        // setup music source
        _musicSource.clip = background;
        _musicSource.loop = true;
        _musicSource.enabled = _playerPrefsManager.MusicPref == 1 ? true : false;
        if (_musicSource.enabled)
        {
            _musicSource.Play();
        }
        // setup sound effect source
        _soundEffectSource.enabled = _playerPrefsManager.SoundPref == 1 ? true : false;
    }

    public bool IsEnabled(AudioType type)
    {
        switch (type)
        {
            case AudioType.Music:
                return _musicSource.enabled;
            case AudioType.Sound:
                return _soundEffectSource.enabled;
            default:
                return false;
        }
    }
}
