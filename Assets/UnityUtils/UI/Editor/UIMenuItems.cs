using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
namespace UnityUtils.UI.Editor
{
    public class UIMenuItems
    {
        [MenuItem("GameObject/UnityUtils/UI/Switch Toggle")]
        public static void CreateSwitchToggle(MenuCommand menuCommand)
        {
            GameObject parent = (GameObject)menuCommand.context;

            GameObject prefab;
            if (parent != null)
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("SwitchToggle"), parent.transform) as GameObject;
            else
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("SwitchToggle")) as GameObject;

            PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            EditorSceneManager.MarkAllScenesDirty();
        }

        [MenuItem("GameObject/UnityUtils/UI/Revive Screen")]
        public static void CreateReviveScreen(MenuCommand menuCommand)
        {
            GameObject parent = (GameObject)menuCommand.context;

            GameObject prefab;
            if (parent != null)
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("ReviveScreen"), parent.transform) as GameObject;
            else
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("ReviveScreen")) as GameObject;

            PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            EditorSceneManager.MarkAllScenesDirty();
        }

        [MenuItem("GameObject/UnityUtils/UI/Rounded Button")]
        public static void CreateRoundedButton(MenuCommand menuCommand)
        {
            GameObject parent = (GameObject)menuCommand.context;

            GameObject prefab;
            if (parent != null)
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("RoundedButton"), parent.transform) as GameObject;
            else
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("RoundedButton")) as GameObject;

            PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            EditorSceneManager.MarkAllScenesDirty();
        }

        [MenuItem("GameObject/UnityUtils/UI/Loading Circle")]
        public static void CreateLoadingCircle(MenuCommand menuCommand)
        {
            GameObject parent = (GameObject)menuCommand.context;

            GameObject prefab;
            if (parent != null)
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("LoadingCircle"), parent.transform) as GameObject;
            else
                prefab = PrefabUtility.InstantiatePrefab(Resources.Load("LoadingCircle")) as GameObject;

            PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
#endif
