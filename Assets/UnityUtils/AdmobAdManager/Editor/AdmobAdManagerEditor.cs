using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace UnityUtils.AdmobAdManager.Editor
{
    [CustomEditor(typeof(AdmobAdManager))]
    public class AdmobAdManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("OPEN EDITOR"))
            {
                AdmobAdManagerWindow.Open();
            }
        }
    }
}
#endif