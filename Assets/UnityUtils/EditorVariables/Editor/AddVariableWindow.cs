using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    public class AddVariableWindow : EditorWindow
    {
        public Vector2 windowSize = new Vector2(350, 155);
        private EditorVariables variables;
        private string key;
        private string[] labels;
        private int selectedLabel;
        private object _object;
        private List<Type> availTypes;
        private string[] availTypeNames;
        private int selected;
        private int previous;
        private bool changed;

        public static void Open(EditorVariables variables)
        {
            AddVariableWindow window = GetWindow<AddVariableWindow>("Add Variable");
            window.minSize = window.windowSize;
            window.maxSize = window.windowSize;

            window.variables = variables;
        }

        private void OnGUI()
        {
            if (variables == null) return;
            EditorUtility.SetDirty(variables);

            if (availTypeNames == null)
            {
                availTypes = EditorVariables.GetAvailableTypes();
                availTypeNames = new string[availTypes.Count];
                for (int i = 0; i < availTypes.Count; i++)
                {
                    availTypeNames[i] = availTypes[i].Name;
                }
                previous = -1;
            }

            selected = EditorGUILayout.Popup("Variable Type", selected, availTypeNames);
            key = EditorGUILayout.TextField("Key", key);

            if (previous != selected)
            {
                changed = true;
                previous = selected;
            }
            else
            {
                changed = false;
            }

            AddEditorGUILayout(availTypes[selected], changed);

            variables.CopyLabels(out labels);
            selectedLabel = EditorGUILayout.Popup("Label", selectedLabel, labels);

            GUILayout.Space(5);
            if (GUILayout.Button("ADD"))
            {
                if (string.IsNullOrEmpty(key) || key.Contains(" "))
                {
                    EditorUtility.DisplayDialog("Warning", "The key cannot be empty or contains space.", "OK");
                    return;
                }
                 
                if (variables.ContainsKey(key))
                {
                    EditorUtility.DisplayDialog("Warning", "The key already in use.", "OK");
                    return;
                }

                variables.AddValue(key, labels[selectedLabel], _object, availTypes[selected]);
                Close();
            }
        }

        private void AddEditorGUILayout(Type type, bool changed)
        {
            if (type == typeof(float))
            {
                if (changed) _object = 0f;
                _object = EditorGUILayout.FloatField("Value", (float)_object);
            }
            else if (type == typeof(Color))
            {
                if (changed) _object = Color.black;
                _object = EditorGUILayout.ColorField("Value", (Color)_object);
            }
            else if (type == typeof(Vector2))
            {
                if (changed) _object = Vector2.zero;
                _object = EditorGUILayout.Vector2Field("Value", (Vector2)_object);
            }
            else if (type == typeof(Vector3))
            {
                if (changed) _object = Vector3.zero;
                _object = EditorGUILayout.Vector3Field("Value", (Vector3)_object);
            }
            else if (type == typeof(Sprite))
            {
                if (changed) _object = null;
                _object = (Sprite)EditorGUILayout.ObjectField("Value", (Sprite)_object, typeof(Sprite), true);
            }
            else if (type == typeof(bool))
            {
                if (changed) _object = false;
                _object = EditorGUILayout.Toggle("Value", (bool)_object);
            }
            else
            {
                throw new Exception("Type is not implemented.");
            }
        }

    }
}
#endif
