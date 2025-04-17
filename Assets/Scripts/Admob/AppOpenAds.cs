
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AppOpenAds : MonoBehaviour
{
    public static AppOpenAds Instance { get; private set; }
    
    [SerializeField] private AdmobSettings settings;
    private AppOpenAd appOpenAd;
    private bool isShowingAd = false;
    private DateTime loadTime;
        private bool isFirstTime = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        string adUnitId = 
        #if UNITY_ANDROID
            settings.AndroidAppOpenId;
        #elif UNITY_IOS
            settings.IOSAppOpenId;  
        #else
            "unexpected_platform";
        #endif

        // Clean up before loading an ad
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        // Create request
        var request = new AdRequest();

        // Load ad
        AppOpenAd.Load(adUnitId, request, (AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError($"Failed to load app open ad: {error.GetMessage()}");
                return;
            }

            appOpenAd = ad;
            loadTime = DateTime.Now;
            
            RegisterEventHandlers(ad);
     
            Debug.Log("App Open ad loaded successfully");
        });
        
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
                isShowingAd = true;
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            isShowingAd = false;
            Debug.Log("App open ad full screen content closed.");
                LoadAd(); 
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
             isShowingAd = false;
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
                            LoadAd();
        };
    }
    public void ShowAdIfAvailable()
    {
            Debug.Log($"AppOpenAds: ShowAdIfAvailable called. IsAdAvailable: {IsAdAvailable}, isShowingAd: {isShowingAd}");
        if (!IsAdAvailable || isShowingAd)
        {
            return;
        }

        appOpenAd.Show();
    }

    private bool IsAdAvailable
    {
        get
        {
            return appOpenAd != null &&
                   (DateTime.Now - loadTime).TotalHours < 4; // Ad expires after 4 hours
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (!paused) // App trở lại foreground
        {
            if (isFirstTime)
            {
                isFirstTime = false; // Bỏ qua lần đầu mở app
                return;
            }
            ShowAdIfAvailable();
        }
    }

}