using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

namespace UnityUtils.AdmobAdManager
{
    public enum BannerSize { Adaptive, Banner, IABBanner, MediumRectangle, Leaderboard }

    public class AdmobAdSettings : ScriptableObject
    {
        [HideInInspector] public bool testAdsEnabled;
        [HideInInspector] public string androidInterstitialAdId;
        [HideInInspector] public string androidRewardedAdId;
        [HideInInspector] public string androidBannerAdId;
        [HideInInspector] public string androidNativeAdId;

        [HideInInspector] public string iosInterstitialAdId;
        [HideInInspector] public string iosRewardedAdId;
        [HideInInspector] public string iosBannerAdId;
        [HideInInspector] public string iosNativeAdId;

        [HideInInspector] public int initializationScene;
        [HideInInspector] public int levelsToShowInterstitialAd;
        [HideInInspector] public bool loadInterstitialAdOnEverySceneLoad;
        [HideInInspector] public bool loadRewardedAdOnEverySceneLoad;
        [HideInInspector] public BannerSize bannerSize;
        [HideInInspector] public bool displayBannerdOnEveryScene;
        [HideInInspector] public List<string> bannerIncludedScenePaths = new List<string>();
        [HideInInspector] public AdPosition bannerPosition;

        private string interstitialTestAdId = "ca-app-pub-5840738909663078/2357383918";
        private string rewardedTestAdId = "ca-app-pub-5840738909663078/7526746275";
        private string bannerTestAdId = "ca-app-pub-5840738909663078/8128585343";
        private string nativeTestAdId = "ca-app-pub-3940256099942544/2247696110";

        public string InterstitialAdId
        {
            get
            {
#if UNITY_ANDROID
            return testAdsEnabled ? interstitialTestAdId : androidInterstitialAdId;
#elif UNITY_IPHONE
            return testAdsEnabled ? interstitialTestAdId : iosInterstitialAdId;
#else
                return "";
#endif
            }
        }
        public string BannerAdId
        {
            get
            {
#if UNITY_ANDROID
            return testAdsEnabled ? bannerTestAdId : androidBannerAdId;
#elif UNITY_IPHONE
            return testAdsEnabled ? bannerTestAdId : iosBannerAdId;
#else
                return "";
#endif
            }
        }
        public string RewardedAdId
        {
            get
            {
#if UNITY_ANDROID
            return testAdsEnabled ? rewardedTestAdId : androidRewardedAdId;
#elif UNITY_IPHONE
            return testAdsEnabled ? rewardedTestAdId : iosRewardedAdId;
#else
                return "";
#endif
            }
        }
        public string NativeAdId
        {
            get
            {
#if UNITY_ANDROID
            return testAdsEnabled ? nativeTestAdId : androidNativeAdId;
#elif UNITY_IPHONE
            return testAdsEnabled ? nativeTestAdId : iosNativeAdId;
#else
                return "";
#endif
            }
        }
    }
}