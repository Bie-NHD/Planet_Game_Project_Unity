using UnityEngine;

[CreateAssetMenu(
    fileName = "BooleanPlayerPrefConfig",
    menuName = "Scriptable Objects/Configs/Player Preferences/Audio Config"
)]
public class AudioPrefConfig : BooleanPlayerPrefConfig
{
    public bool IsLoop = false;

    public AudioSource AudioSource;

    public override void SetPref(bool value)
    {
        base.SetPref(value);
        if (AudioSource != null)
        {
            AudioSource.enabled = value;
        }
        else
        {
            Debug.LogError($"AudioPrefConfig: AudioSource is null for key {Key}.");
        }
    }
}
