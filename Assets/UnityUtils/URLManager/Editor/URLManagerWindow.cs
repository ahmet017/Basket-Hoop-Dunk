using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.URLManager.Editor
{
    public class URLManagerWindow : EditorWindow
    {
        private URLSettings settings;
        private SerializedObject serializedObject;
        SerializedProperty policyUrls;

        [MenuItem("Game/URL Manager")]
        public static void Open()
        {
            URLManagerWindow window = GetWindow<URLManagerWindow>("URL Manager");
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Warning", "Use this editor in edit mode. Changes you make in play mode may not be applied.", "OK");
            }
        }
        private void OnEnable()
        {
            settings = AssetDatabase.LoadAssetAtPath<URLSettings>("Assets/UnityUtils/URLManager/Runtime/Resources/URLSettings.asset");
            if (settings == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<URLSettings>(), "Assets/UnityUtils/URLManager/Runtime/Resources/URLSettings.asset");
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
            settings = AssetDatabase.LoadAssetAtPath<URLSettings>("Assets/UnityUtils/URLManager/Runtime/Resources/URLSettings.asset");
        }

        private void OnGUI()
        {
            EditorUtility.SetDirty(settings);
            serializedObject = new SerializedObject(settings);
            serializedObject.Update();

            settings.androidStoreUrl = EditorGUILayout.TextField("Android Store URL", settings.androidStoreUrl);
            settings.iosStoreUrl = EditorGUILayout.TextField("IOS Store URL", settings.iosStoreUrl);
            settings.privacyPolicyUrl = EditorGUILayout.TextField("Privacy Policy URL", settings.privacyPolicyUrl);

            GUILayout.Space(30);
            EditorGUILayout.LabelField("Privacy Policy for Specific Languages");
            policyUrls = serializedObject.FindProperty("privacyPolicyUrls");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                policyUrls.InsertArrayElementAtIndex(policyUrls.arraySize);
            }
            if (GUILayout.Button("-"))
            {
                if (policyUrls.arraySize > 0)
                    policyUrls.DeleteArrayElementAtIndex(policyUrls.arraySize - 1);
            }
            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < policyUrls.arraySize; i++)
            {
                EditorGUILayout.PropertyField(policyUrls.GetArrayElementAtIndex(i).FindPropertyRelative("language"));
                EditorGUILayout.PropertyField(policyUrls.GetArrayElementAtIndex(i).FindPropertyRelative("url"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif