using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityUtils.Editor
{
    public class SceneChooserWindow : EditorWindow
    {
        private int sceneCount;
        private string[] sceneNames;
        private int selectedIndex;
        public UnityAction<string> onSelected; 

        public static void Open(UnityAction<string> onSelected)
        {
            SceneChooserWindow window = GetWindow<SceneChooserWindow>("Choose Scene");
            Vector2 size = new Vector2(400, 80);
            window.minSize = size;
            window.maxSize = size;

            window.onSelected = onSelected;
        }

        private void OnGUI()
        {
            if (onSelected == null) return;

            sceneCount = EditorSceneManager.sceneCountInBuildSettings; 
            if (sceneCount == 0) return;

            sceneNames = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames[i] = SceneUtility.GetScenePathByBuildIndex(i);
            }

            GUILayout.Space(10);
            selectedIndex = EditorGUILayout.Popup("Scene", selectedIndex, sceneNames);

            GUILayout.Space(10);
            if (GUILayout.Button("CHOOSE"))
            {
                onSelected.Invoke(sceneNames[selectedIndex]);
                Close();
            }
        }
    }
}
#endif