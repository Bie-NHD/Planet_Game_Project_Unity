using UnityEngine;

/// <summary>
/// A configuration class for managing audio-related player preferences.
/// Inherits from <see cref="BooleanPlayerPrefConfig"/>.
/// </summary>
[CreateAssetMenu(
    fileName = "BooleanPlayerPrefConfig",
    menuName = "Scriptable Objects/Configs/Player Preferences/Audio Config"
)]
public class AudioPrefConfig : BooleanPlayerPrefConfig
{
    /// <summary>
    /// Indicates whether the audio should loop.
    /// </summary>
    public bool IsLoop = false;

    /// <summary>
    /// The <see cref="AudioSource"/> associated with this configuration.
    /// </summary>
    public AudioSource AudioSource;

    /// <summary>
    /// Sets the player preference value and updates the associated <see cref="AudioSource"/>.
    /// </summary>
    /// <param name="value">The boolean value to set for the player preference.</param>
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
