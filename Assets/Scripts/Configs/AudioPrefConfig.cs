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
        AudioSource.enabled = value;
    }
}
