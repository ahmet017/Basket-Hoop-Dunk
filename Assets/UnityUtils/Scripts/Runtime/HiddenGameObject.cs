using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityUtils
{
    [ExecuteInEditMode]
    public class HiddenGameObject : MonoBehaviour
    {
        public static readonly string HIDE_OBJECTS_KEY = "HideGameObjects";

        public static bool IsObjectsHidden
        {
            get
            {
#if UNITY_EDITOR
                return EditorPrefs.GetBool(HIDE_OBJECTS_KEY, true);
#else   
                return false;
#endif

            }
            set
            {
#if UNITY_EDITOR
                EditorPrefs.SetBool(HIDE_OBJECTS_KEY, value);
#endif

            }
        }

        private void OnValidate()
        {
            RefreshHiddenState();
        }

        public void RefreshHiddenState()
        {
#if UNITY_EDITOR
            bool hidden = IsObjectsHidden;
            gameObject.hideFlags = hidden ? HideFlags.HideInHierarchy : HideFlags.None;
#endif
        }
    }
}

