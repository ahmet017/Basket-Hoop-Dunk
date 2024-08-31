using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    public class EditKeyWindow : EditorWindow
    {
        private EditorVariables variables;
        private string key;
        private string[] labels;
        private int selectedLabel;
        private UnityAction<string, string> callback;
        private Vector2 windowSize = new Vector2(400, 70);

        public static void Open(EditorVariables variables, UnityAction<string, string> callback)
        {
            EditKeyWindow window = GetWindow<EditKeyWindow>("Edit Key");
            window.minSize = window.windowSize;
            window.maxSize = window.windowSize;

            window.callback = callback;
            window.variables = variables;
        }

        private void OnGUI()
        {
            if (variables == null) return;

            key = EditorGUILayout.TextField("New Key", key);

            if (labels == null)
                variables.CopyLabels(out labels);
            selectedLabel = EditorGUILayout.Popup("New Label", selectedLabel, labels);

            if (GUILayout.Button("CHANGE"))
            {
                if (!string.IsNullOrEmpty(key) && key.Contains(" "))
                {
                    EditorUtility.DisplayDialog("Warning", "The key cannot be empty or contains space.", "OK");
                    return;
                }

                if (!string.IsNullOrEmpty(key) && variables.ContainsKey(key))
                {
                    EditorUtility.DisplayDialog("Warning", "The key already in use.", "OK");
                    return;
                }

                callback.Invoke(key, labels[selectedLabel]);
                Close();
            }
        }
    }
}
#endif

