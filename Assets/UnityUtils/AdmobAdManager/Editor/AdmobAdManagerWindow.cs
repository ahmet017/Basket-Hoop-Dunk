using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityUtils.Editor;

namespace UnityUtils.AdmobAdManager.Editor
{
    public class AdmobAdManagerWindow : EditorWindow
    {
        private readonly string file = "Assets/Resources/AdmobAdSettings.asset";
        private AdmobAdSettings settings;
        private int labelWidth = 250;
        private int bannerScenesCount;
        private Vector2 scrollPos;
        private string levelsToShowInterstitialTooltip = "This value determines how many levels should be played before showing the interstitial ad.";

        [MenuItem("Game/Admob Ad Manager")]
        public static void Open()
        {
            AdmobAdManagerWindow window = GetWindow<AdmobAdManagerWindow>("Admob Ad Manager");
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Warning", "Use this editor in edit mode. Changes you make in play mode may not be applied.", "OK");
            }
        }

        private void OnEnable()
        {
            settings = AssetDatabase.LoadAssetAtPath<AdmobAdSettings>(file);
            if (settings == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources"); 
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AdmobAdSettings>(), file);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                settings = AssetDatabase.LoadAssetAtPath<AdmobAdSettings>(file);
            }
        }

        private void OnGUI()
        {
            EditorUtility.SetDirty(settings);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            GUILayout.Space(10);
            GUILayout.Label("Ad Ids", EditorStyles.boldLabel);
            settings.androidInterstitialAdId = EditorGUILayout.TextField("Android Interstitial Ad ID", settings.androidInterstitialAdId);
            settings.androidRewardedAdId = EditorGUILayout.TextField("Android Rewarded Ad ID", settings.androidRewardedAdId);
            settings.androidBannerAdId = EditorGUILayout.TextField("Android Banner Ad ID", settings.androidBannerAdId);
            settings.androidNativeAdId = EditorGUILayout.TextField("Android Native Ad ID", settings.androidNativeAdId);
            GUILayout.Space(5);
            settings.iosInterstitialAdId = EditorGUILayout.TextField("IOS Interstitial Ad ID", settings.iosInterstitialAdId);
            settings.iosRewardedAdId = EditorGUILayout.TextField("IOS Rewarded Ad ID", settings.iosRewardedAdId);
            settings.iosBannerAdId = EditorGUILayout.TextField("IOS Banner Ad ID", settings.iosBannerAdId);
            settings.iosNativeAdId = EditorGUILayout.TextField("IOS Native Ad ID", settings.iosNativeAdId);

            GUILayout.Space(30);
            GUILayout.Label("Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Initialization Scene Build Index", GUILayout.Width(labelWidth));
            settings.initializationScene = EditorGUILayout.IntField(settings.initializationScene);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Levels to Show Interstitial Ad", levelsToShowInterstitialTooltip), GUILayout.Width(labelWidth));
            settings.levelsToShowInterstitialAd = EditorGUILayout.IntField(settings.levelsToShowInterstitialAd);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Load Interstitial Ad On Every Scene Load", GUILayout.Width(labelWidth));
            settings.loadInterstitialAdOnEverySceneLoad = EditorGUILayout.Toggle(settings.loadInterstitialAdOnEverySceneLoad);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Load Rewarded Ad On Every Scene Load", GUILayout.Width(labelWidth));
            settings.loadRewardedAdOnEverySceneLoad = EditorGUILayout.Toggle(settings.loadRewardedAdOnEverySceneLoad);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Banner Size", GUILayout.Width(labelWidth));
            settings.bannerSize = (BannerSize)EditorGUILayout.EnumPopup(settings.bannerSize);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Banner Position", GUILayout.Width(labelWidth));
            settings.bannerPosition = (GoogleMobileAds.Api.AdPosition)EditorGUILayout.EnumPopup(settings.bannerPosition);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Display Banner On Every Scene", GUILayout.Width(labelWidth));
            settings.displayBannerdOnEveryScene = EditorGUILayout.Toggle(settings.displayBannerdOnEveryScene);
            EditorGUILayout.EndHorizontal();

            if (!settings.displayBannerdOnEveryScene)
            {
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Banner Scenes", EditorStyles.boldLabel);
                bannerScenesCount = settings.bannerIncludedScenePaths.Count;
                for (int i = 0; i < bannerScenesCount; i++)
                {
                    if (i >= settings.bannerIncludedScenePaths.Count) break;
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", GUILayout.Width(50)))
                    {
                        settings.bannerIncludedScenePaths.Remove(settings.bannerIncludedScenePaths[i]);
                    }
                    if (i >= settings.bannerIncludedScenePaths.Count)
                    {
                        EditorGUILayout.EndHorizontal();
                        break;
                    };
                    GUILayout.Space(5);
                    GUI.Label(GUILayoutUtility.GetRect(new GUIContent(settings.bannerIncludedScenePaths[i]), "label"), settings.bannerIncludedScenePaths[i]);
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("+"))
                {
                    SceneChooserWindow.Open((scenePath) =>
                    {
                        settings.bannerIncludedScenePaths.Add(scenePath);
                    });
                }
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(30);
            GUILayout.Label("Test Ads", EditorStyles.boldLabel);
            settings.testAdsEnabled = EditorGUILayout.Toggle("Test Ads Enabled", settings.testAdsEnabled);
            if (settings.testAdsEnabled)
            {
                EditorGUILayout.HelpBox("Don't forget to disable test ads in the release version.", MessageType.Warning);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}


#endif
