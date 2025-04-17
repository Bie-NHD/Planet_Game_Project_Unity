using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterstitialAds : MonoBehaviour
{
    [SerializeField] private AdmobSettings settings;
    
    private InterstitialAd interstitialAd;
    private bool isLoading;
    private Action onAdClosed;

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
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Interstitial ad paid {adValue.Value} {adValue.CurrencyCode}");
        };

        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad opening.");
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad closed.");
            LoadAd(); // Load the next interstitial ad
            
            // Invoke callback if exists
            onAdClosed?.Invoke();
            onAdClosed = null;
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"Interstitial ad failed to open full screen content with error: {error.GetMessage()}");
            LoadAd(); // Try to load another ad
            
            // Invoke callback if exists since we won't show the ad
            onAdClosed?.Invoke();
            onAdClosed = null;
        };
    }

    public void ShowAd(Action onClose = null)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            onAdClosed = onClose;
            interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial ad not ready yet.");
            onClose?.Invoke(); // Call the callback immediately if ad isn't ready
            LoadAd(); // Try to load a new ad for next time
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
