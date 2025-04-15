using UnityEngine;
using Utils;

[CreateAssetMenu(
    fileName = "BooleanPlayerPrefConfig",
    menuName = "Scriptable Objects/Configs/Player Preferences/Boolean Config"
)]
public class BooleanPlayerPrefConfig : BasePlayerPrefConfig<bool>
{
    [SerializeField]
    private bool IsEnabled = true;

    public new string Key;

    void Awake()
    {
        if (!PlayerPrefs.HasKey(Key))
        {
            SetPref(IsEnabled);
        }
        else
        {
            IsEnabled = MathUtils.Clamp01ToBool(PlayerPrefs.GetInt(Key));
        }
    }

    public override bool GetPref()
    {
        return IsEnabled;
    }

    public override void SetPref(bool value)
    {
        PlayerPrefs.SetInt(Key, MathUtils.BoolToInt(value));
        PlayerPrefs.Save(); // TODO: Check performance impact
        IsEnabled = value;
    }

    public void Enable()
    {
        IsEnabled = true;
        SetPref(IsEnabled);
    }

    public void Disable()
    {
        IsEnabled = false;
        SetPref(IsEnabled);
    }

    public void TogglePref()
    {
        IsEnabled = !IsEnabled;
        SetPref(IsEnabled);
    }
}
