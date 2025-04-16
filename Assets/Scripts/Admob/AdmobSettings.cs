using UnityEngine;

[CreateAssetMenu(fileName = "AdmobSettings", menuName = "Ads/Admob Settings")]
public class AdmobSettings : ScriptableObject
{
    [Header("Android")]
    public string AndroidAppId;
    public string AndroidBannerId;
    public string AndroidInterstitialId;
    public string AndroidRewardedId;

    [Header("iOS")]
    public string IOSAppId;
    public string IOSBannerId;
    public string IOSInterstitialId;
    public string IOSRewardedId;

    public string GetAppId()
    {
        #if UNITY_ANDROID
            return AndroidAppId;
        #elif UNITY_IOS
            return IOSAppId;
        #else
            return "";
        #endif
    }
}
