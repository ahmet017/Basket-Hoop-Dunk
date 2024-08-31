using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
namespace UnityUtils.AdmobAdManager.Editor
{
    public class AdmobAdManagerMenuItems
    {
        [MenuItem("GameObject/UnityUtils/Admob Ad Manager")]
        public static void CreateAdManager()
        {
            GameObject go = new GameObject("AdmobAdManager");
            go.AddComponent<AdmobAdManager>();
            EditorSceneManager.MarkAllScenesDirty();
        }

        [MenuItem("GameObject/UnityUtils/UMP Manager")]
        public static void CreateUMPManager()
        {
            GameObject go = new GameObject("UMPManager");
            go.AddComponent<AdmobUMPManager>();
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}

#endif