using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.NotificationManager.Editor
{
    [CustomEditor(typeof(NotificationManager))]
    public class NotificationManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("OPEN EDITOR"))
            {
                NotificationManagerWindow.Open();
            }
        }
    }
}

#endif