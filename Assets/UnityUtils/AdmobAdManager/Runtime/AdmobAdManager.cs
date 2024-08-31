using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UnityUtils.AdmobAdManager
{
    public class AdmobAdManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            AdmobAdManager.settings = Resources.Load<AdmobAdSettings>("AdmobAdSettings");
            SceneManager.activeSceneChanged += OnActiveSceneChanged_Initialization;
        }

        public static void OnActiveSceneChanged_Initialization(Scene scene1, Scene scene2)
        {
            if (scene2.buildIndex >= settings.initializationScene)
            {
                Instantiate(Resources.Load("AdmobAdManager"));
                SceneManager.activeSceneChanged -= OnActiveSceneChanged_Initialization;
            }
        }

        private static AdmobAdSettings settings;
        private static bool interstitialAdsDisabled;
        private static bool bannerAdsDisabled;

        private BannerView bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
        private bool rewardedAdLoading;
        private bool interstitialAdLoading;

        public int LevelsToShowInterstitialAd => settings.levelsToShowInterstitialAd;

        private static AdmobAdManager instance = null;
        public static AdmobAdManager Instance
        {
            get { return instance; }
        }
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            MobileAds.Initialize(initStatus => { });

            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoaded;  
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (settings.loadInterstitialAdOnEverySceneLoad && !interstitialAdsDisabled)
                RequestAndLoadInterstitialAd();
            if (settings.loadRewardedAdOnEverySceneLoad)
                RequestAndLoadRewardedAd();

            if (settings.displayBannerdOnEveryScene || settings.bannerIncludedScenePaths.Contains(scene.path))
            {
                if (bannerView == null && !bannerAdsDisabled)
                {
                    RequestAndLoadBanner();
                }
            }
            else
            {
                if (bannerView != null)
                {
                    bannerView.Destroy();
                    bannerView = null;
                }
            }
        }

        private AdRequest GetAdRequest()
        {
            AdRequest request = new AdRequest();
            return request;
        }


        #region Banner
        public void RequestAndLoadBanner()
        {
            string adUnitId = settings.BannerAdId;

            AdSize adSize;
            switch (settings.bannerSize)
            {
                case BannerSize.Adaptive: adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth); break;
                case BannerSize.Banner: adSize = AdSize.Banner; break;
                case BannerSize.IABBanner: adSize = AdSize.IABBanner; break;
                case BannerSize.Leaderboard: adSize = AdSize.Leaderboard; break;
                case BannerSize.MediumRectangle: adSize = AdSize.MediumRectangle; break;
                default: adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth); break;
            }

            bannerView = new BannerView(adUnitId, adSize, settings.bannerPosition);

            bannerView.OnBannerAdLoaded += () =>
            {
                //Debug.Log("Ad loaded , " + bannerView.GetHeightInPixels());
            };

            AdRequest request = GetAdRequest();
            bannerView.LoadAd(request);

        }
        #endregion


        #region Rewarded Ad
        /// <summary>
        /// Loads a rewarded ad if it isn't currently loading or hasn't loaded yet.
        /// </summary>
        public void RequestAndLoadRewardedAd()
        {
            string adUnitId = settings.RewardedAdId;

            if (rewardedAdLoading || (rewardedAd != null && rewardedAd.CanShowAd())) return;

            DestroyRewardedAd();

            rewardedAdLoading = true;
            AdRequest request = GetAdRequest();

            RewardedAd.Load(adUnitId, request, (RewardedAd ad, LoadAdError error) =>
            {
                if (ad != null)
                {
                    rewardedAd = ad;
                    Debug.Log("Rewarded ad loaded.");
                }
                else
                {
                    Debug.Log("Rewarded ad failed to load with error: " + error.GetMessage());
                }
                rewardedAdLoading = false;
            });
        }
        public void ShowRewardedAdIfLoaded(UnityAction<bool> callback)
        {
            if (rewardedAd.CanShowAd())
            {
                rewardedAd.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Rewarded ad opening.");
                };
                rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.Log("Rewarded ad failed to show.");
                    callback.Invoke(false);
                    DestroyRewardedAd();
                    RequestAndLoadRewardedAd();
                };
                rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Rewarded ad closed.");
                    DestroyRewardedAd();
                    RequestAndLoadRewardedAd();
                };

                rewardedAd.Show((Reward reward) =>
                {
                    callback.Invoke(true);
                });
            }
        }
        private void DestroyRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
        }
        public bool IsRewardedAdReady()
        {
            return rewardedAd != null && rewardedAd.CanShowAd();
        }
        #endregion


        #region Interstitial Ad
        /// <summary>
        /// Loads an insterstitial ad if it isn't currently loading or hasn't loaded yet.
        /// </summary>
        public void RequestAndLoadInterstitialAd()
        {
            string adUnitId = settings.InterstitialAdId;

            if (interstitialAdLoading || (interstitialAd != null && interstitialAd.CanShowAd())) return;


            DestroyInterstitialAd();

            interstitialAdLoading = true;
            AdRequest request = GetAdRequest();

            InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
            {
                if (ad != null)
                {
                    interstitialAd = ad;
                    Debug.Log("Interstitial ad loaded.");
                }
                else
                {
                    Debug.Log("Interstitial ad failed to load with error: " + error.GetMessage());
                }
                interstitialAdLoading = false;
            });
        }

        /// <summary>
        /// Shows the interstitial ad if loaded.
        /// </summary>
        /// <param name="callback">Returns a bool value indicating the ad was opened or failed.</param>
        public void ShowInterstitialAdIfLoaded(UnityAction<bool> callback)
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                interstitialAd.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Interstitial ad opening.");
                };
                interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    DestroyInterstitialAd();
                    RequestAndLoadInterstitialAd();
                    callback.Invoke(true);
                };
                interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.Log("Interstitial ad failed to show.");
                    DestroyInterstitialAd();
                    callback.Invoke(false);
                };
                interstitialAd.Show();
            }
            else
            {
                callback.Invoke(false);
                DestroyInterstitialAd();
                RequestAndLoadInterstitialAd();
            }

        }
        private void DestroyInterstitialAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
        }
        #endregion


        /// <summary>
        /// Load requests for interstitial ads will be ignored after this function is called.
        /// </summary>
        public static void DisableInterstitialAds()
        {
            interstitialAdsDisabled = true;
        }

        /// <summary>
        /// Load requests for banner ads will be ignored after this function is called.
        /// </summary>
        public static void DisableBannerAds()
        {
            bannerAdsDisabled = true;
        }

    }
}

