using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.GamePromotion.Editor
{
    public class GamePromotionManagerWindow : EditorWindow
    {
        int selectedGame;
        private GUIStyle selectedButtonStyle;
        private GUIStyle normalButtonStyle;
        private Vector2 scrollPos;
        private GamePromotionSettings settings;

        [MenuItem("Game/Game Promotion Manager")]
        public static void Open()
        {
            GamePromotionManagerWindow window = GetWindow<GamePromotionManagerWindow>("Game Promotion");
            window.minSize = new Vector2(500, 600);
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Warning", "Use this editor in edit mode. Changes you make in play mode may not be applied.", "OK");
            }
        }
        private void OnEnable()
        {
            settings = AssetDatabase.LoadAssetAtPath<GamePromotionSettings>("Assets/UnityUtils/GamePromotion/Runtime/Resources/GamePromotionSettings.asset");
            if (settings == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GamePromotionSettings>(),
                    "Assets/UnityUtils/GamePromotion/Runtime/Resources/GamePromotionSettings.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                settings = AssetDatabase.LoadAssetAtPath<GamePromotionSettings>("Assets/UnityUtils/GamePromotion/Runtime/Resources/GamePromotionSettings.asset");
            }
        }

        private void OnGUI()
        {
            EditorUtility.SetDirty(settings);

            if (selectedButtonStyle == null)
            {
                selectedButtonStyle = new GUIStyle(GUI.skin.button);
                selectedButtonStyle.normal.textColor = Color.white;
            }
            if (normalButtonStyle == null)
            {
                normalButtonStyle = new GUIStyle(GUI.skin.button);
                normalButtonStyle.normal.textColor = Color.gray;
            }

            EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawSideBar();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();


            for (int i = 0; i < settings.otherGames.Count; i++)
            {
                if (i == selectedGame)
                {
                    settings.otherGames[i].androidStoreUrl = EditorGUILayout.TextField("Android Store URL", settings.otherGames[i].androidStoreUrl);
                    settings.otherGames[i].iosStoreUrl = EditorGUILayout.TextField("IOS Store URL", settings.otherGames[i].iosStoreUrl);
                    settings.otherGames[i].image = (Sprite)EditorGUILayout.ObjectField("Promotion Image (16:9)", settings.otherGames[i].image, typeof(Sprite), true);
                    //  EditorGUILayout.HelpBox("The aspect ratio of the promotion image should be 9:16.", MessageType.Info);

                    if (settings.otherGames[i] != null && settings.otherGames[i].image != null && (settings.otherGames[i].image.texture.width / settings.otherGames[i].image.texture.height) != (16 / 9))
                    {
                        settings.otherGames[i].image = null;
                        EditorUtility.DisplayDialog("Incorrect Dimension", "The aspect ratio of the promotion image should be 16:9.", "OK");
                    }
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

        }

        private void DrawSideBar()
        {
            for (int i = 0; i < settings.otherGames.Count; i++)
            {
                if (GUILayout.Button("Game_" + (i + 1), i == selectedGame ? selectedButtonStyle : normalButtonStyle))
                {
                    selectedGame = i;
                    GUIUtility.keyboardControl = 0;
                    GUIUtility.hotControl = 0;
                }

            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                settings.otherGames.Add(new OtherGame());
            }
            if (GUILayout.Button("-"))
            {
                if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete the Game_" + (selectedGame + 1) + " ?", "YES", "NO"))
                {
                    if (settings.otherGames.Count > 0)
                    {
                        settings.otherGames.RemoveAt(selectedGame);
                    }

                }
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}
#endif

