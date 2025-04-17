    using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSDK();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSDK()
    {
        MobileAds.Initialize(initStatus => {
            Debug.Log("AdMob SDK initialized");
        });
    }
}
