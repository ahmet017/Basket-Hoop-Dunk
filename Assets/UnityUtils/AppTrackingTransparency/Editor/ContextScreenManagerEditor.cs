using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityUtils.Editor;
namespace UnityUtils.AppTrackingTransparency.Editor
{
    [CustomEditor(typeof(ContextScreenManager))]
    public class ContextScreenManagerEditor : UnityEditor.Editor
    {
        private ContextScreenManager screenManager;

        private void OnEnable()
        {
            screenManager = (ContextScreenManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("contextScreenPrefab"));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(screenManager.scenePath, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("CHOOSE SCENE"))
            {
                SceneChooserWindow.Open((scenePath) =>
                {
                    screenManager.scenePath = scenePath;
                });
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(screenManager);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

}
#endif