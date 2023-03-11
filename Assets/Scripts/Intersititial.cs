using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Intersititial : MonoBehaviour
{
    // Start is called before the first frame update
    public InterstitialAd interstitial;

    private void Start()
    {

        MobileAds.Initialize(InitializationStatus => { });
        RequestInterstitial();
        

    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
    }
}
