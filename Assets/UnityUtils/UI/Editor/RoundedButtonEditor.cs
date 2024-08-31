using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.UI.Editor
{
    [CustomEditor(typeof(RoundedButton))]
    public class RoundedButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Click on GameObject > UI > Rounded Button to create a rounded button.", MessageType.Info);
            EditorGUILayout.HelpBox("There are some hidden objects under the rounded button. You can reveal them by " +
                "clicking on GameObject > Show Hidden Objects and Fields.", MessageType.Info);
            base.OnInspectorGUI();
            if (GUI.changed)
                EditorUtility.SetDirty((RoundedButton)target);
        }
    }
}
#endif
