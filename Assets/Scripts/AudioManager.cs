using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----------Audio Sources----------")]
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _soundEffectSource;
    [Header("-----------Audio Clip----------")]
    public AudioClip background;
    public AudioClip thow;
    public AudioClip merge;

    public AudioClip gameOver;

    private void Start()
    {
        _musicSource.clip = background;
        _musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (_soundEffectSource.enabled == false)
        {
            return;
        }
        _soundEffectSource.PlayOneShot(clip);
    }

    public void ToggleMusic(bool isOn)
    {
        if (_musicSource.enabled == false)
        {
            Debug.LogWarning("AudioManager: Music source is disabled. Cannot pause music.");
            return;
        }
        switch (isOn)
        {
            case true:
                _musicSource.UnPause();
                break;
            case false:
                _musicSource.Pause();
                break;
        }
    }

}
