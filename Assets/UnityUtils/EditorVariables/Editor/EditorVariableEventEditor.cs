using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorVariableEvent<>), true)]
    public class EditorVariableEventEditor<T> : UnityEditor.Editor
    {
        private EditorVariableEvent<T> mEvent;
        private List<Type> typesCanBeModified = new List<Type> { typeof(Color), typeof(float), typeof(Vector2), typeof(Vector3) };

        private void OnEnable()
        {
            mEvent = (EditorVariableEvent<T>)target;
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(mEvent, "Editor Variables");
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal(); 
            if (GUILayout.Button("EDIT KEY", GUILayout.Width(100)))
            {
                KeyChooserWindow.Open<T>((key) =>
                {
                    mEvent.key = key; 
                });
            }
            EditorGUILayout.LabelField("Key : " + mEvent.key);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Event"));

            if (typesCanBeModified.Contains(typeof(T)))
            {
                mEvent.modifierEnabed = EditorGUILayout.Toggle("Modifier Enabled", mEvent.modifierEnabed);
                if (mEvent.modifierEnabed)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("modifier"));
                }
            }

            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck()) 
            {
                EditorUtility.SetDirty(mEvent);
                SetEventSettigs();
            }
        }
         
        private void SetEventSettigs()
        {
            for (int i = 0; i < mEvent.Event.GetPersistentEventCount(); i++)
            {
                mEvent.Event.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
                if (mEvent.Event.GetPersistentTarget(i) != null)
                    EditorUtility.SetDirty(mEvent.Event.GetPersistentTarget(i));
            }

        }
    }
}
#endif

