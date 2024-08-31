using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.URLManager.Editor
{
    [CustomEditor(typeof(StoreButton))]
    public class StoreButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("OPEN EDITOR"))
            {
                URLManagerWindow.Open();
            }
        }
    }
}
#endif