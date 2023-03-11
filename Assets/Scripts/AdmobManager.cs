using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoBehaviour
{
    private InterstitialAd Interstitial;
    private RewardedAd rewardedAd;
    public bool isErned;


    public void RequestInterstitial()
    {
        string AdId;
#if UNITY_ANDROID
        AdId = "ca-app-pub-3940256099942544/1033173712";
#else
                      AdId = "ca-app-pub-3940256099942544/1033173712";
#endif

        Interstitial = new InterstitialAd(AdId);
        AdRequest request = new AdRequest.Builder().Build();
        Interstitial.LoadAd(request);

        Interstitial.OnAdClosed += InterstitialClosed;

    }

    void InterstitialClosed(object sender, EventArgs args)
    {
        Interstitial.Destroy();
        RequestInterstitial();
    }

    public void ShowInterstitial()
    {
        if (Interstitial.IsLoaded())
        {
            Interstitial.Show();
        }
        else
        {
            Interstitial.Destroy();
            RequestInterstitial();
        }
    }

    public void RequestRewarded()
    {
        string AdId;
#if UNITY_ANDROID
        AdId = "	ca-app-pub-3940256099942544/5224354917";
#else
                        AdId = "	ca-app-pub-3940256099942544/5224354917";
#endif

        rewardedAd = new RewardedAd(AdId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        rewardedAd.OnAdClosed += RewardedClosed;
        rewardedAd.OnUserEarnedReward += RewardedCompleted;

      

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    void RewardedClosed(object sender, EventArgs e)
    {
        RequestRewarded();
    }
    void RewardedCompleted(object sender, Reward e)
    {
        // ödül alýndý
        isErned = true;
     


    }
    public void showRewarded()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: ");

    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        isErned = true;
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

    }





}
