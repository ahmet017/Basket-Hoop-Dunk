using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityUtils.EditorVariables
{
    [ExecuteInEditMode]
    public abstract class EditorVariableEvent<T> : MonoBehaviour, IEditorVariableEvent<T>
    {
        public string key;
        public UnityEvent<T> Event;
        public System.Type Type { get { return typeof(T); } }

        public bool modifierEnabed;
        public EventModifier<T> modifier;

        public virtual void Invoke(T value)
        {
#if UNITY_EDITOR
            for (int i = 0; i < Event.GetPersistentEventCount(); i++)
            {
                EditorUtility.SetDirty(Event.GetPersistentTarget(i));
            }
#endif
        }
    }
}

