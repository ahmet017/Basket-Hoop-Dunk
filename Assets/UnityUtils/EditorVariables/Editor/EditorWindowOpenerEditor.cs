using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorWindowOpener))]
    public class EditorWindowOpenerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Use this editor to reskin the game.", MessageType.Info);

            if (GUILayout.Button("OPEN EDITOR"))
                EditorVariablesWindow.Open();
        }
    }
}

#endif