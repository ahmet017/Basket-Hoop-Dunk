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
    [CustomEditor(typeof(AdmobUMPManager))]
    public class AdmobUMPManagerEditor : UnityEditor.Editor
    {
        private AdmobUMPManager umpManager;

        public override void OnInspectorGUI()
        {
            umpManager = (AdmobUMPManager)target;
            serializedObject.Update();

            umpManager.underAge = EditorGUILayout.Toggle("Under Age Consent", umpManager.underAge);
            umpManager.loadSceneAfterComplete = EditorGUILayout.Toggle("Load A Scene After Complete", umpManager.loadSceneAfterComplete);
            if (umpManager.loadSceneAfterComplete)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel, GUILayout.Width(100));
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField(umpManager.scenePath, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("CHOOSE SCENE"))
                {
                    SceneChooserWindow.Open((scenePath) =>
                    {
                        umpManager.scenePath = scenePath;
                    });
                }
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onComplete"));
            }

            

            if (GUI.changed)
            {
                EditorUtility.SetDirty(umpManager);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
