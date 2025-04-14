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

    [SerializeField]
    private new string _key = string.Empty;

    public string Key => _key;

    void Awake()
    {
        if (!PlayerPrefs.HasKey(_key))
        {
            SetPref(IsEnabled);
        }
        else
        {
            IsEnabled = MathUtils.Clamp01ToBool(PlayerPrefs.GetInt(_key));
        }
    }

    public override bool GetPref()
    {
        return IsEnabled;
    }

    public override void SetPref(bool value)
    {
        PlayerPrefs.SetInt(_key, MathUtils.BoolToInt(value));
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
