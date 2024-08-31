using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SkinManager))]
public class SkinManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("OPEN EDITOR"))
        {
            SkinManagerWindow.Open();
        }
    }
}
#endif
