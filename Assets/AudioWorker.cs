using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioWorker : MonoBehaviour
{
    public AudioPrefConfig AudioPrefConfig;

    [SerializeField]
    public AudioSource AudioSource;

    public bool IsEnabled => AudioSource.enabled || AudioPrefConfig.GetPref();

    public string Key => AudioPrefConfig.Key;

    void Start()
    {
        AudioPrefConfig.AudioSource = AudioSource;
        AudioSource.enabled = AudioPrefConfig.GetPref();
        AudioSource.loop = AudioPrefConfig.IsLoop;
    }

    public void ToggleAudio() => AudioPrefConfig.TogglePref();

    public void Play() => AudioSource.Play();

    public void PlayOneShot(AudioClip clip)
    {
        if (IsEnabled)
        {
            AudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"AudioWorker: {Key} is disabled. Cannot play audio clip.");
        }
    }

    public void Stop() => AudioSource.Stop();

    public void Pause() => AudioSource.Pause();

    public void UnPause() => AudioSource.UnPause();

    public bool IsPlaying => IsEnabled && AudioSource.isPlaying;

    public void Enable()
    {
        AudioSource.enabled = true;
        AudioPrefConfig.Enable();
    }

    public void Disable()
    {
        AudioSource.enabled = false;
        AudioPrefConfig.Disable();
    }

    public override string ToString()
    {
        return $"AudioWorker: {Key}\tIsEnabled: {IsEnabled}";
    }
}
