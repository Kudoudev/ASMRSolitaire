using System;
using System.Collections.Generic;
// using AudienceNetwork;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IronSourceManager : SingletonMonoBehaviour<IronSourceManager>
{
    private const string TAG = "[IRON_SOURCE]";
    private const string KeyRemovedInterstitialAds = "iron_source_manager_removed_interstitial_ads";

    [SerializeField] private string ironSourceKeyAndroid;
    [SerializeField] private string ironSourceKeyIos;

    private string isKey;
    private bool _isFinishVideo;
    private bool _isShowingAds;

    public void Start()
    {
#if UNITY_ANDROID
        isKey = ironSourceKeyAndroid;
#else
            isKey = ironSourceKeyIos;
#endif
        IronSource.Agent.init(isKey);
        IronSource.Agent.validateIntegration();

        if (!IsRemovedInterstitialAds())
        {
            IronSource.Agent.loadInterstitial();
            IronSourceInterstitialEvents.onAdOpenedEvent += OnInterstitialOpenEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += OnInterstitialClosedEvent;
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }

        IronSourceRewardedVideoEvents.onAdOpenedEvent += OnRewardedOpenEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += OnRewardedClosed;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += OnRewardedCompleted;

        lastShowInter = Time.time; 
        lastShowReward = Time.time;
    }

    private void OnRewardedOpenEvent(IronSourceAdInfo adInfo)
    {
        _isShowingAds = true;
    }

    private void OnRewardedClosed(IronSourceAdInfo adInfo)
    {
        // IronSource.Agent.displayBanner();
        lastShowReward = Time.time;
        _isShowingAds = false;

        if (_isFinishVideo)
        {
            _onRewardedComplete?.Invoke();
        }
    }

    private void OnRewardedCompleted(IronSourcePlacement obj, IronSourceAdInfo adInfo)
    {
        _isFinishVideo = true;
    }

    private void OnInterstitialOpenEvent(IronSourceAdInfo adInfo)
    {
        _isShowingAds = true;
    }

    private void OnInterstitialClosedEvent(IronSourceAdInfo adInfo)
    {
        _isShowingAds = false;

        lastShowInter = Time.time;

        // IronSource.Agent.displayBanner();
        IronSource.Agent.loadInterstitial();

        _onInterstitialClosed?.Invoke();
    }

    /// <summary>
    /// hiển thị quảng cáo intertitial
    /// </summary>
    /// <param name="onInterstitialClosed">gọi khi user xem hết ads</param>
    /// <param name="showAdsResult">trả về kết quả show ads</param>
    /// <param name="where">vị trí đặt ads</param>
    /// <param name="level">level cao nhất của user</param>
    private static float TIME_SHOW_ADS = 60f;

    private static float TIME_SHOW_ADS_AFTER_REWARD = 30f;

    [HideInInspector] public float lastShowInter = 0, lastShowReward = 0;

    public void ShowInterstitialAd(Action onInterstitialClosed = null)
    {
        if (IsRemovedInterstitialAds())
        {
            onInterstitialClosed?.Invoke();
            return;
        }

        if (IsInterstitialAdsAvailable())
        {
            if (Time.time - lastShowReward < TIME_SHOW_ADS_AFTER_REWARD)
            {
                onInterstitialClosed?.Invoke();
                return;
            }

            if (Time.time - lastShowInter < TIME_SHOW_ADS)
            {
                onInterstitialClosed?.Invoke();
                return;
            }

            IronSource.Agent.showInterstitial();

            _onInterstitialClosed = onInterstitialClosed;

            if (!_isShowingAds)
            {
                // showAdsResult?.Invoke(ShowAdsResult.Success);
            }
        }
        else
        {
            onInterstitialClosed?.Invoke();
            // showAdsResult?.Invoke(ShowAdsResult.AdsNotAvailable);
            IronSource.Agent.loadInterstitial();
        }
    }

    private Action _onRewardedComplete;
    private Action _onInterstitialClosed;

    /// <summary>
    /// hiển thị quảng cáo rewarded 
    /// </summary>
    /// <param name="onRewardedComplete">gọi khi user xem hết video</param>
    /// <param name="showAdsResult">trả về kết quả show ads</param>
    /// <param name="where">vị trí đặt ads</param>
    /// <param name="level">level cao nhất của user</param>
    /// <param name="showRewardInstead"></param>
    public void ShowRewardedAds(Action onRewardedComplete)
    {
#if UNITY_EDITOR
        onRewardedComplete?.Invoke();
        return;
#endif

        if (!IsRewardedVideoAvailable())
        {
            // PopupManager.Instance.ShowNoVideoPopup();

            return;
        }

        _onRewardedComplete = onRewardedComplete;

        _isFinishVideo = false;

        IronSource.Agent.showRewardedVideo();
    }

    /// <summary>
    /// set flag để xem xét có hiển thị interstitial ads cho user hay ko
    /// 0 = vẫn show interstitial ads
    /// 1 = ko show interstitial ads
    /// </summary>
    /// <param name="flag"></param>
    public void SetRemoveInterstitialAds(int flag)
    {
        PlayerPrefs.SetInt(KeyRemovedInterstitialAds, flag);
        IronSource.Agent.hideBanner();
    }


    /// <summary>
    /// kiểm tra user có mua gói bỏ ads ko
    /// 0 = chưa mua; 1 = mua
    /// </summary>
    /// <returns></returns>
    public bool IsRemovedInterstitialAds()
    {
        if (!PlayerPrefs.HasKey(KeyRemovedInterstitialAds))
        {
            return false;
        }

        return PlayerPrefs.GetInt(KeyRemovedInterstitialAds) == 1;
    }

    /// <summary>
    /// kiểm tra có video interstitial để hiển thị hay ko
    /// </summary>
    /// <returns></returns>
    public bool IsInterstitialAdsAvailable()
    {
        return IronSource.Agent.isInterstitialReady();
    }

    /// <summary>
    /// kiểm tra có reward video để hiển thị hay ko
    /// </summary>
    /// <returns></returns>
    public bool IsRewardedVideoAvailable()
    {
        return IronSource.Agent.isRewardedVideoAvailable();
    }

    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }
// #endif
}

public enum ShowAdsResult
{
    AdsNotAvailable,
    RemovedInterstitial,
    Success
}