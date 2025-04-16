using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;

public class BannerAds : MonoBehaviour
{
    [SerializeField] private AdmobSettings settings;
     [SerializeField] private RectTransform adPlaceholder; 
      [SerializeField] private TextMeshProUGUI loadingText;
    private BannerView bannerView;

    private void Start()
    { 
        LoadBanner();
    }
 
    private void LoadBanner()
    {
        string adUnitId = 
        #if UNITY_ANDROID
            settings.AndroidBannerId;
#elif UNITY_IOS
            settings.IOSBannerId;
#else
            "unexpected_platform";
#endif
       
        int screenWidth = (int)Screen.width;
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(screenWidth);

        // Tạo banner với kích thước và vị trí
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
       
        RegisterEventHandlers();
        // Tạo 
        var adRequest = new AdRequest();
        
        // Load banner
        bannerView.LoadAd(adRequest);
    }

   private void RegisterEventHandlers()
    {
        // Fired when an ad is loaded into the banner view
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded successfully");
              HideLoadingText();
                ShowBanner();
        };

        // Fired when an ad fails to load into the banner view
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError($"Banner ad failed to load with error: {error.GetMessage()}");
            HideLoadingText();
            HideBanner();
        };

        // Fired when the ad is clicked
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("Banner ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode);
            Debug.Log(msg);
        };

        // Fired when an impression is recorded for an ad
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner ad recorded an impression.");
        };

        // Fired when the ad is clicked
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner ad was clicked.");
        };

        // Fired when an ad opened full screen content
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner ad full screen content opened.");
        };

        // Fired when the ad closed full screen content
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner ad full screen content closed.");
        };
    }
   private void ShowLoadingText()
    {
        if (loadingText != null)
        {
            loadingText.text = "Loading ads...";
            loadingText.gameObject.SetActive(true);
        }
       
    }

    private void HideLoadingText()
    {
        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(false);
        }
    }

    public void ShowBanner()
    {
        bannerView?.Show();
        
    }

    public void HideBanner()
    {
        bannerView?.Hide();
        
    }

    private void OnDestroy()
    {
        bannerView?.Destroy();
        bannerView = null;
    }
        public void RetryLoadBanner()
    {
        bannerView?.Destroy();
        bannerView = null;
        LoadBanner();
    }
}
