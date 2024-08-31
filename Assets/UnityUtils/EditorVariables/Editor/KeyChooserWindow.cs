using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    public class KeyChooserWindow : EditorWindow
    {
        public UnityAction<string> callback;

        private EditorVariables variables;
        private string[] keys;
        private Vector2 scrollPos;

        public static void Open<T>(UnityAction<string> callback)
        {
            KeyChooserWindow window = GetWindow<KeyChooserWindow>("Choose Key");

            window.callback = callback;
            window.variables = AssetDatabase.LoadAssetAtPath<EditorVariables>(EditorVariablesWindow.path);
            window.variables.CopyKeys<T>(out window.keys);
        }

        private void OnGUI()
        {
            if (keys == null || variables == null) return;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginVertical();

            foreach (string key in keys)
            {
                if (GUILayout.Button(key))
                {
                    callback.Invoke(key);
                    Close();
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif
