using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
namespace UnityUtils.NotificationManager.Editor
{
    public class NotificationManagerMenuItem
    {
        [MenuItem("GameObject/UnityUtils/Notification Manager")]
        public static void CreateObject()
        {
            GameObject go = new GameObject("NotificationManager");
            go.AddComponent<NotificationManager>();
            EditorSceneManager.MarkAllScenesDirty();
        }
    }

}
#endif
