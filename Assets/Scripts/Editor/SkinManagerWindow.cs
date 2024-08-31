using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class SkinManagerWindow : EditorWindow
{
    private enum SkinManagerTab { BallSkins, LineSkins }

    private SkinSettings settings;
    private SkinManagerTab selectedTab;
    private int selectedBallSkin;
    private int selectedLineSkin;
    private List<SkinManagerTab> tabs = new List<SkinManagerTab>() { SkinManagerTab.BallSkins, SkinManagerTab.LineSkins };
    private GUIStyle selectedButtonStyle;
    private GUIStyle normalButtonStyle;
    private Vector2 scrollPos;

    [MenuItem("Game/Skin Manager")]
    public static void Open()
    {
        SkinManagerWindow window = GetWindow<SkinManagerWindow>("Skin Manager");
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Warning", "Use this editor in edit mode. Changes you make in play mode may not be applied.", "OK");
        }
    }

    private void OnEnable()
    {
        settings = AssetDatabase.LoadAssetAtPath<SkinSettings>("Assets/SkinSettings.asset");
        if (settings == null)
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SkinSettings>(), "Assets/SkinSettings.asset");
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            settings = AssetDatabase.LoadAssetAtPath<SkinSettings>("Assets/SkinSettings.asset");
        }
        selectedTab = SkinManagerTab.BallSkins;

        if (settings.ballSkins.Count == 0)
        {
            settings.ballSkins.Add(new BallSkin());
        }
        if (settings.lineSkins.Count == 0)
        {
            settings.lineSkins.Add(new LineSkin());
        }
        selectedBallSkin = 0;
        selectedLineSkin = 0;
    }

    private void OnGUI()
    {
        EditorUtility.SetDirty(settings);
        if (normalButtonStyle == null)
        {
            normalButtonStyle = new GUIStyle(GUI.skin.button);
            normalButtonStyle.normal.textColor = Color.gray;
        }
        if (selectedButtonStyle == null)
        {
            selectedButtonStyle = new GUIStyle(GUI.skin.button);
            selectedButtonStyle.normal.textColor = Color.white;
        }

        settings.unlockAllSkins = EditorGUILayout.Toggle("Unlock All Skins", settings.unlockAllSkins);
        if (settings.unlockAllSkins)
            EditorGUILayout.HelpBox("All skins are unlocked. Disable this setting in release version.", MessageType.Warning);

        EditorGUILayout.BeginHorizontal();
        foreach (SkinManagerTab tab in tabs)
        {
            if (GUILayout.Button(tab.ToString(), tab == selectedTab ? selectedButtonStyle : normalButtonStyle))
            {
                selectedTab = tab;
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        DrawSideBar();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        DrawProporties();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSideBar()
    {
        if (selectedTab == SkinManagerTab.BallSkins)
        {
            for (int i = 0; i < settings.ballSkins.Count; i++)
            {
                if (GUILayout.Button("BallSkin_" + (i + 1), i == selectedBallSkin ? selectedButtonStyle : normalButtonStyle))
                {
                    selectedBallSkin = i;
                    GUIUtility.keyboardControl = 0;
                    GUIUtility.hotControl = 0;
                }
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                settings.ballSkins.Add(new BallSkin());
            }
            if (GUILayout.Button("-"))
            {
                if (settings.ballSkins.Count > 1)
                {
                    if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete the BallSkin_" + (selectedBallSkin + 1) + " ?", "YES", "NO"))
                    {
                        settings.ballSkins.RemoveAt(selectedBallSkin);
                        if (selectedBallSkin >= settings.ballSkins.Count)
                            selectedBallSkin = settings.ballSkins.Count - 1;
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Warning", "You can't delete the last one. A ball skin is needed to run the game.", "OK");
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            for (int i = 0; i < settings.lineSkins.Count; i++)
            {
                if (GUILayout.Button("LineSkin_" + (i + 1), i == selectedLineSkin ? selectedButtonStyle : normalButtonStyle))
                {
                    selectedLineSkin = i;
                    GUIUtility.keyboardControl = 0;
                    GUIUtility.hotControl = 0;
                }
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                settings.lineSkins.Add(new LineSkin());
            }
            if (GUILayout.Button("-"))
            {
                if (settings.lineSkins.Count > 1)
                {
                    if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete the LineSkin_" + (selectedLineSkin + 1) + " ?", "YES", "NO"))
                    {
                        settings.lineSkins.RemoveAt(selectedLineSkin);
                        if (selectedLineSkin >= settings.lineSkins.Count)
                            selectedLineSkin = settings.lineSkins.Count - 1;
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Warning", "You can't delete the last one. A line skin is needed to run the game.", "OK");
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        

    }

    private void DrawProporties()
    {
        if (selectedTab == SkinManagerTab.BallSkins)
        {
            settings.ballSkins[selectedBallSkin].sprite = (Sprite)EditorGUILayout.ObjectField("Ball Sprite", settings.ballSkins[selectedBallSkin].sprite, typeof(Sprite), true);
        }
        else
        {
            settings.lineSkins[selectedLineSkin].color = EditorGUILayout.ColorField("Line Color", settings.lineSkins[selectedLineSkin].color);

        }
    }



}
#endif