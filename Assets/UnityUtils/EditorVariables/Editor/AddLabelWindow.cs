using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    public class AddLabelWindow : EditorWindow
    {
        public Vector2 windowSize = new Vector2(350, 70);
        public EditorVariables variables;
        private string label;

        public static void Open(EditorVariables variables)
        {
            AddLabelWindow window = GetWindow<AddLabelWindow>("Add Label");
            window.minSize = window.windowSize;
            window.maxSize = window.windowSize;

            window.variables = variables;
        }

        private void OnGUI()
        {
            if (variables == null) return;

            label = EditorGUILayout.TextField("Label", label);

            if (GUILayout.Button("ADD"))
            {
                if (string.IsNullOrEmpty(label))
                {
                    EditorUtility.DisplayDialog("Warning", "The label cannot be null.", "OK");
                    return;
                }

                if (variables.labels.ContainsKey(label))
                {
                    EditorUtility.DisplayDialog("Warning", "The label already in use.", "OK");
                    return;
                }

                variables.labels.Add(label, new Label());
                Close();
            }
        }
    }
}
#endif
