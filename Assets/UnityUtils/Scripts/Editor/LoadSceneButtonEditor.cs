using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityUtils.Editor
{
    [CustomEditor(typeof(LoadSceneButton))]
    public class LoadSceneButtonEditor : UnityEditor.Editor
    {
        private LoadSceneButton loadSceneButton;

        public override void OnInspectorGUI()
        {
            loadSceneButton = (LoadSceneButton)target;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(loadSceneButton.scenePath, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
           
            if (GUILayout.Button("CHOOSE SCENE"))
            {
                SceneChooserWindow.Open((scenePath) =>
                {
                    loadSceneButton.scenePath = scenePath;
                });
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(loadSceneButton);
            }
        }
    }
}
#endif