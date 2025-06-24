using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class ISManager : MonoBehaviour
{
    static public ISManager I;

    private LevelPlayInterstitialAd interstitial;
    private LevelPlayRewardedAd rewarded;
    public string androidKey, iOSKey;
    public string androidInterstitial, androidRewarded;
    public string iosInterstitial, iosRewarded;


    void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Auto Init events
        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += OnInitFailed;

        string key = "";
#if UNITY_ANDROID
        key = androidKey;
#else
        key = iOSKey;
#endif
        LevelPlay.Init(key);

    }

    string interstitialAD
    {
        get
        {
#if UNITY_ANDROID
            return androidInterstitial;
#else
            return iosInterstitial;
#endif
        }
    }

    string rewardedVideoAD
    {
        get
        {
#if UNITY_ANDROID
            return androidRewarded;
#else
            return iosRewarded;
#endif
        }
    }

    void OnInitSuccess(LevelPlayConfiguration config)
    {
        Debug.Log("SDK initialized via Auto Init");

        interstitial = new LevelPlayInterstitialAd(interstitialAD);
        interstitial.OnAdLoaded += info => Debug.Log("Interstitial loaded");
        interstitial.OnAdClosed += info => { Debug.Log("Interstitial closed — loading next"); interstitial.LoadAd(); };
        interstitial.OnAdLoadFailed += err => Debug.LogWarning("Interstitial failed: " + err.ErrorMessage);

        rewarded = new LevelPlayRewardedAd(rewardedVideoAD);
        rewarded.OnAdLoaded += info => Debug.Log("Rewarded loaded");
        rewarded.OnAdRewarded += (info, reward) => GrantReward(reward.Name, reward.Amount);
        rewarded.OnAdClosed += info => { Debug.Log("Rewarded closed — loading next"); rewarded.LoadAd(); };
        rewarded.OnAdLoadFailed += err => Debug.LogWarning("Rewarded failed: " + err.ErrorMessage);

        interstitial.LoadAd();
        rewarded.LoadAd();
    }

    void OnInitFailed(LevelPlayInitError err)
    {
        Debug.LogError("SDK init failed: " + err.ErrorMessage);
    }

    void OnApplicationPause(bool paused)
    {
        LevelPlay.SetPauseGame(paused);
    }

    public void ShowInterstitial()
    {
        if (Data.purchased)
        {
            Debug.Log("Subscription is active, not showing interstitial.");
            return;
        }
        if (interstitial != null && interstitial.IsAdReady())
        {
            Debug.Log("Show interstitial");
            interstitial.ShowAd();
        }
        else
        {
            Debug.Log("Interstitial not ready, loading...");
            if (interstitial != null)
            {
                interstitial.LoadAd();
            }
        }
    }

    private System.Action currentRewardCallback;

    public void ShowRewarded(System.Action onRewardGranted = null)
    {
        if (rewarded != null && rewarded.IsAdReady())
        {
            currentRewardCallback = onRewardGranted;
            rewarded.ShowAd();
        }
        else
        {
            Debug.Log("Rewarded not ready, loading...");
            if (rewarded != null)
            {
                rewarded.LoadAd();
            }
        }
    }

    public bool IsRewardReady => rewarded != null && rewarded.IsAdReady();

    private void GrantReward(string name, int amount)
    {
        Debug.Log($"Reward granted: {amount} {name}");

        // Call the callback if one was provided
        currentRewardCallback?.Invoke();

        // Reset the callback
        currentRewardCallback = null;
    }
    void OnDestroy()
    {
        // Clean up event subscriptions
        LevelPlay.OnInitSuccess -= OnInitSuccess;
        LevelPlay.OnInitFailed -= OnInitFailed;
    }
}