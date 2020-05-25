using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System.Net;

public class Ads_Manager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    string testDeviceId_iOS = "63f69b14516e82df33cc2eced3e8cc1c";
    string testAdId_iOS = "ca-app-pub-3940256099942544/1712485313";

    string idAd_iOS = "ca-app-pub-8891462778088936/4527428711";
    string idAd_Android = "ca-app-pub-8891462778088936/2004179485";

    #region Init

    string URL = "https://www.google.com";
    bool hasConnection = false;
    public bool HasConnection
    {
        get { return hasConnection; }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Application.isEditor) { return; }


        CheckConnection();
    }

    void InitAds()
    {
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

        StartCoroutine("WaitToLoad");
    }

    IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MenuScene");
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
        string adUnitId = idAd_Android;
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
        //Debug.Log("Ad Loading Called " + Time.time);
        rewardedAd.LoadAd(CreateAdRequest());
    }

    #endregion


    public void ShowRewardedAd()
    {        
        if (rewardedAd != null && rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Didnt Show Ad");        
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
        Analytics.CustomEvent("AdFailedToLoad");

        GameObject menuGO = GameObject.FindGameObjectWithTag("MenuManager");
        if (menuGO)
        {
            menuGO.GetComponent<MenuManager>().AdFinished();
        }
        else
        {
            Debug.Log("Could not find Menu Manager");
        }
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("Ad Opened");
        Analytics.CustomEvent("AdOpened");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Ad Failed to Open " + args.Message);
        Analytics.CustomEvent("AdFailedToShow");

        GameObject menuGO = GameObject.FindGameObjectWithTag("MenuManager");
        if (menuGO)
        {
            menuGO.GetComponent<MenuManager>().AdFinished();
        }
        else
        {
            Debug.Log("Could not find Menu Manager");
        }
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad Closed");
        //Have to load new ad object to be able to display new ad
        Analytics.CustomEvent("AdClosed");
        RequestAndLoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("Ad Finished " + amount.ToString() + " " + type);
        Analytics.CustomEvent("AdFinished");

        GameObject menuGO = GameObject.FindGameObjectWithTag("MenuManager");

        if (menuGO)
        {
            menuGO.GetComponent<MenuManager>().AdFinished();
        }
        else
        {
            Debug.Log("Could not find Menu Manager");
        }
    }

    #endregion

    
    public void CheckConnection()
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Timeout = 5000;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                hasConnection = true;
                InitAds();
            }
            else
            {
                hasConnection = false;
                Debug.Log("No Internet Connection 1");
                SceneManager.LoadScene("MenuScene");
            }
        }
        catch
        {
            hasConnection = false;
            Debug.Log("No Internet Connection 2");
            SceneManager.LoadScene("MenuScene");
        }
    }

}



