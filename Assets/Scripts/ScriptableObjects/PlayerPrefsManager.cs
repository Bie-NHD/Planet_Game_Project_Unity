using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsKeys
{
    public const string MusicPref = "MusicPref";
    public const string SoundPref = "SoundPref";
}

enum BinaryState
{
    Off = 0,
    On = 1,
}

[CreateAssetMenu(
    fileName = "PlayerPrefsManager",
    menuName = "Scriptable Objects/PlayerPrefsManager",
    order = 1
)]
public class PlayerPrefsManager : ScriptableObject
{
    private Dictionary<string, int> m_playerPrefs = new Dictionary<string, int>();

    public int MusicPref
    {
        get => GetPlayerPrefInt(PlayerPrefsKeys.MusicPref, 1);
        set => SetPlayerPrefInt(PlayerPrefsKeys.MusicPref, value);
    }

    public int SoundPref
    {
        get => GetPlayerPrefInt(PlayerPrefsKeys.SoundPref, 1);
        set => SetPlayerPrefInt(PlayerPrefsKeys.SoundPref, value);
    }

    void Awake()
    {
        SetupPlayerPrefs();
    }

    private void SetupPlayerPrefs()
    {
        GetPlayerPrefInt(PlayerPrefsKeys.MusicPref, 1);
        GetPlayerPrefInt(PlayerPrefsKeys.SoundPref, 1);
    }

    private int GetPlayerPrefInt(string key, int defaultValue)
    {
        if (m_playerPrefs.ContainsKey(key))
        {
            return m_playerPrefs[key];
        }

        if (PlayerPrefs.HasKey(key))
        {
            m_playerPrefs[key] = PlayerPrefs.GetInt(key);
            return m_playerPrefs[key];
        }
        else
        {
            PlayerPrefs.SetInt(key, defaultValue);
            m_playerPrefs[key] = defaultValue;
            return defaultValue;
        }
    }

    public void TogglePlayerPref(String key, bool isOn)
    {
        SetPlayerPrefInt(key, isOn ? 1 : 0);
    }

    private void SetPlayerPrefInt(string key, int value)
    {
        value = Mathf.Clamp(value, 0, 1); // Ensure value is between 0 and 1
        m_playerPrefs[key] = value;
        PlayerPrefs.SetInt(key, value);
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
