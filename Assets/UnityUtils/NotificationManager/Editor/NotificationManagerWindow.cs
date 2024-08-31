using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

namespace UnityUtils.NotificationManager.Editor
{
    public class NotificationManagerWindow : EditorWindow
    {
        private string file = "Assets/Resources/NotificationSettings.asset";
        private NotificationSettings settings;
        private SerializedObject serializedObject;
        private Vector2 scrollPos;

        [MenuItem("Game/Notification Manager")]
        public static void Open()
        {
            NotificationManagerWindow window = GetWindow<NotificationManagerWindow>("Notification Manager");
        }

        private void OnEnable()
        {
            settings = AssetDatabase.LoadAssetAtPath<NotificationSettings>(file);
            if (settings == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NotificationSettings>(), file);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                settings = AssetDatabase.LoadAssetAtPath<NotificationSettings>(file);
            }
            
        }

        private void OnGUI()
        {
            EditorUtility.SetDirty(settings);
            serializedObject = new SerializedObject(settings);
            serializedObject.Update();

            settings.notificationsEnabled = EditorGUILayout.Toggle("Notifications Enabled", settings.notificationsEnabled);

            if (settings.notificationsEnabled)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                settings.notificationTestingEnabled = EditorGUILayout.Toggle("Notification Testing Enabled", settings.notificationTestingEnabled);
                if (settings.notificationTestingEnabled)
                    EditorGUILayout.HelpBox("Notifications will be shown 10 seconds after the game is opened if the notification testing enabled. " +
                        "Don't forget to disable it in the release version.", MessageType.Warning);

                for (int i = 0; i < settings.notifications.Count; i++)
                {
                    GUILayout.Space(15);
                    EditorGUILayout.BeginHorizontal("box");

                    if (GUILayout.Button("-", EditorStyles.miniButtonMid))
                    {
                        settings.notifications.RemoveAt(i);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndScrollView();
                        GUIUtility.keyboardControl = 0;
                        GUIUtility.hotControl = 0;
                        return;
                    }

                    EditorGUILayout.BeginVertical();
                    settings.notifications[i].format = (NotificationFormat)EditorGUILayout.EnumPopup("Notification Format", settings.notifications[i].format);
                    if (settings.notifications[i].format == NotificationFormat.String)
                    {
                        settings.notifications[i].titleString = EditorGUILayout.TextField("Title", settings.notifications[i].titleString);
                        settings.notifications[i].messageString = EditorGUILayout.TextField("Message", settings.notifications[i].messageString);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("notifications").GetArrayElementAtIndex(i).FindPropertyRelative("titleLocalized"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("notifications").GetArrayElementAtIndex(i).FindPropertyRelative("messageLocalized"));
                    }
                    settings.notifications[i].duration = EditorGUILayout.IntField("Duration (hours)", settings.notifications[i].duration);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(15);
                if (GUILayout.Button("ADD NOTIFICATION"))
                {
                    settings.notifications.Add(new Notification());
                }
                EditorGUILayout.EndScrollView();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}


#endif


