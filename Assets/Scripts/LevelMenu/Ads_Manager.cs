using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class Ads_Manager : MonoBehaviour
{
    MenuManager menuManager;

    private RewardedAd rewardedAd;

    string testDeviceId_iOS = "63f69b14516e82df33cc2eced3e8cc1c";
    string testAdId_iOS = "ca-app-pub-3940256099942544/1712485313";
    string idAd_iOS = "ca-app-pub-8891462778088936/4527428711";

    #region Init

    void Start()
    {
        if(Application.isEditor) { return; }

        menuManager = GetComponent<MenuManager>();

        MobileAds.SetiOSAppPauseOnBackground(true);

        List<string> deviceIds = new List<string>() { AdRequest.TestDeviceSimulator };

        // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add(testDeviceId_iOS);
#elif UNITY_ANDROID
        deviceIds.Add("75EF8D155528C04DACBBA6F36F433035");
#endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            //.SetTestDeviceIds(deviceIds)
            .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
    }

    void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            Debug.Log("Initialization complete");
            RequestAndLoadRewardedAd();
        });
    }

    AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()        
            //.AddTestDevice(testDeviceId_iOS)         
            .TagForChildDirectedTreatment(false) 
            .Build();
    }

    public void RequestAndLoadRewardedAd()
    {

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string adUnitId = idAd_iOS;
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    #endregion


    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad is NULL");
            menuManager.AdFinished();
        }
    }



    #region Events

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad Loaded");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Ad Failed to load " + args.Message);
        menuManager.AdFinished();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("Ad Opened");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Ad Failed to Open " + args.Message);
        menuManager.AdFinished();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad Closed");
        //Have to load new ad object to be able to display new ad
        RequestAndLoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("Ad Finished " + amount.ToString() + " " + type);

        menuManager.AdFinished();
    }

    #endregion

}





/*   

    /*
    void LoadAdObject()
    {
        string adUnitId;

#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-8891462778088936/4527428711";
#else
            adUnitId = "unexpected_platform";
#endif       

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        List<string> deviceIds = new List<string>();
        deviceIds.Add("63f69b14516e82df33cc2eced3e8cc1c");
        RequestConfiguration.Builder requestConfigurationBuilder = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds);

        MobileAds.SetRequestConfiguration(requestConfigurationBuilder.build());

        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

 
 void OnApplicationPause(bool isPaused)
 {
     IronSource.Agent.onApplicationPause(isPaused);
 }
        Debug.Log("unity-script: ShowRewardedVideoButtonClicked");
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }

 */
