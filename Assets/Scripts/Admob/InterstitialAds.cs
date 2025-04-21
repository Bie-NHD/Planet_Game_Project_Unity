using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class InterstitialAds : MonoBehaviour
{
    [SerializeField] private AdmobSettings settings;
    
    private InterstitialAd interstitialAd;
    private bool isLoading;

    private Action onAdClosedCallback;
    public bool IsShowingAd { get; private set; }

    private void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        if (isLoading) return;

        isLoading = true;

        string adUnitId = 
        #if UNITY_ANDROID
            settings.AndroidInterstitialId;
        #elif UNITY_IOS
            settings.IOSInterstitialId;
        #else
            "unexpected_platform";
        #endif

        // Clean up before loading an ad
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading interstitial ad...");

        var request = new AdRequest();
        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            isLoading = false;

            if (error != null || ad == null)
            {
                Debug.LogError($"Interstitial ad failed to load with error: {error?.GetMessage()}");
                return;
            }

            interstitialAd = ad;
            RegisterEventHandlers(ad);
            Debug.Log("Interstitial ad loaded successfully");
        });
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {  IsShowingAd = true;
            Debug.Log("Interstitial ad opening.");
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
              IsShowingAd = false;
            Debug.Log("Interstitial ad closed.");
            LoadAd(); // Load the next interstitial ad

            UnityMainThreadDispatcher.Enqueue(() => {
                onAdClosedCallback?.Invoke();
                onAdClosedCallback = null;
            });
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
              IsShowingAd = false;
            Debug.LogError($"Interstitial ad failed to open full screen content with error: {error.GetMessage()}");
            LoadAd(); // Try to load another ad
                      // Call on main thread
            UnityMainThreadDispatcher.Enqueue(() => {
                onAdClosedCallback?.Invoke();
                onAdClosedCallback = null;
            });
        };
    }

    public void ShowAd(Action callback)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            IsShowingAd = true;
            onAdClosedCallback = callback;
            interstitialAd.Show();
        }
        else
        {
            callback?.Invoke();
        }
    }


    private void OnDestroy()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
    }
}
