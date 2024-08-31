using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
namespace UnityUtils.EditorVariables.Editor
{
    public class EditorVariablesWindow : EditorWindow
    {
        public static readonly string path = "Assets/UnityUtils/EditorVariables/Runtime/EditorVariables.asset";
        private EditorVariables variables;
        private string selectedLabel;
        private string[] labels;
        private GUIStyle selectedButtonStyle;
        private GUIStyle normalButtonStyle;
        private Vector2 scrollPosLabels;
        private Vector2 scrollPosVariables;
        private string[] keys;
        private string[] scenePaths;
        private int sceneCount;
        private Scene scene;
        private Scene activeScene;
        private string activeScenePath;
        private GameObject[] allGameObjects;
        private List<Component> derivedComponents;
        private Component[] components;
        private Type componentType;
        private Type genericArg;
        private MethodInfo invokeMethod;
        private FieldInfo keyField;
        private Type variablesClassType = typeof(EditorVariables);
        private MethodInfo genericGetValueMethod;
        private object value;
        private Type keyType;
        private bool replace;
        private string replaceKey;
        private char menuIcon = '\u2630';


        [MenuItem("Game/Editor Variables")]
        public static void Open()
        {
            EditorVariablesWindow window = GetWindow<EditorVariablesWindow>("Editor Variables");
            window.minSize = Vector2.right * 600 + Vector2.up * 200;

            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Warning", "Don't use this editor in play mode!", "OK");
            }
        }
         
        private void OnEnable()
        {
            variables = AssetDatabase.LoadAssetAtPath<EditorVariables>(path);
            if (variables == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<EditorVariables>(), path);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                variables = AssetDatabase.LoadAssetAtPath<EditorVariables>(path);
            }
            if (variables.labels.Count == 0)
            {
                variables.labels.Add(EditorVariables.DEFAULT_LABEL, new Label());
            }
            selectedLabel = EditorVariables.DEFAULT_LABEL;
        }

        private void OnLostFocus()
        {
            replace = false;
        }

        private void OnGUI()
        {
            EditorUtility.SetDirty(variables);
            Undo.RecordObject(variables, "Editor Variables");

            if (selectedButtonStyle == null || normalButtonStyle == null)
            {
                selectedButtonStyle = new GUIStyle(GUI.skin.button);
                selectedButtonStyle.normal.textColor = Color.white;

                normalButtonStyle = new GUIStyle(GUI.skin.button);
                normalButtonStyle.normal.textColor = Color.gray;
            }

            if (GUILayout.Button("+"))
            {
                AddVariableWindow.Open(variables);
            }
            if (GUILayout.Button("APPLY CHANGES"))
            {
                activeScene = EditorSceneManager.GetActiveScene();
                EditorSceneManager.SaveOpenScenes();
                sceneCount = EditorSceneManager.sceneCountInBuildSettings;
                scenePaths = new string[sceneCount];
                for (int i = 0; i < sceneCount; i++)
                {
                    scenePaths[i] = SceneUtility.GetScenePathByBuildIndex(i);
                    if (i == activeScene.buildIndex)
                        activeScenePath = scenePaths[i];
                }

                foreach (string sceneName in scenePaths)
                {
                    EditorSceneManager.OpenScene(sceneName);
                    scene = EditorSceneManager.GetActiveScene();

                    allGameObjects = FindObjectsOfType<GameObject>(true);
                    derivedComponents = new List<Component>();


                    foreach (GameObject obj in allGameObjects)
                    {
                        components = obj.GetComponents<Component>();

                        foreach (var component in components)
                        {
                            if (component == null) continue;
                            componentType = component.GetType();

                            if (IsDerivedFromGenericType(componentType, typeof(EditorVariableEvent<>)))
                            {
                                derivedComponents.Add(component);
                            }
                        }
                    }

                    foreach (Component comp in derivedComponents)
                    {
                        componentType = comp.GetType();
                        genericArg = componentType.BaseType.GetGenericArguments()[0];
                        invokeMethod = GetInvokeMethod(componentType, genericArg);
                        if (invokeMethod == null) continue;
                        keyField = componentType.GetField("key");

                        genericGetValueMethod = variablesClassType.GetMethod("GetValue").MakeGenericMethod(genericArg);
                        value = genericGetValueMethod.Invoke(variables, new object[] { (string)keyField.GetValue(comp) });

                        if (value == default) continue;

                        if (invokeMethod != null && invokeMethod.GetParameters().Length == 1)
                        {
                            invokeMethod.Invoke(comp, new object[] { value });
                        }
                    }

                    EditorSceneManager.SaveScene(scene);
                }
                EditorSceneManager.OpenScene(activeScenePath);
            }
            EditorGUILayout.HelpBox("Changes are only applied to the scenes in the build settings.", MessageType.Info);
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box", GUILayout.Width(150), GUILayout.ExpandHeight(true));
            scrollPosLabels = EditorGUILayout.BeginScrollView(scrollPosLabels);

            variables.CopyLabels(out labels);
            foreach (string label in labels)
            {
                if (GUILayout.Button(label, label == selectedLabel ? selectedButtonStyle : normalButtonStyle))
                {
                    selectedLabel = label;
                }
            }

            EditorGUILayout.BeginHorizontal();
            // ADD LABEL
            if (GUILayout.Button("+"))
            {
                AddLabelWindow.Open(variables);
            }
            // REMOVE LABEL
            if (GUILayout.Button("-"))
            {
                if (selectedLabel == EditorVariables.DEFAULT_LABEL)
                {
                    EditorUtility.DisplayDialog("Warning", "You can't delete the default label.", "OK");
                }
                else if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to dele te label '" + selectedLabel + "' ?", "YES", "NO"))
                {
                    variables.RemoveLabel(selectedLabel);
                    selectedLabel = EditorVariables.DEFAULT_LABEL;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            scrollPosVariables = EditorGUILayout.BeginScrollView(scrollPosVariables);

            GetKeys();
            foreach (string key in keys)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(menuIcon.ToString(), (replace && key == replaceKey) ? selectedButtonStyle : normalButtonStyle, 
                    GUILayout.Width(25)))
                {
                    if (!replace)
                    {
                        replace = true;
                        replaceKey = key;
                    }else
                    {
                        variables.ReplaceKeys(selectedLabel, replaceKey, key);
                        replace = false;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndScrollView();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                }
                if (GUILayout.Button("-", GUILayout.Width(50)))
                {
                    if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete the '" + key + "' ?", "YES", "NO"))
                    {
                        variables.RemoveKey(key);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndScrollView();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                }
                if (GUILayout.Button("EDIT", GUILayout.Width(50)))
                {
                    EditKeyWindow.Open(variables, (newKey, newLabel) =>
                    {
                        if (string.IsNullOrEmpty(newKey)) newKey = key;
                        variables.UpdateVariable(key, newKey, newLabel);
                        UpdateKeyInVariableEvents(key, newKey);
                    });
                } 
                GUILayout.Label(key, GUILayout.Width(200));
                keyType = variables.GetTypeOfTheKey(key);
                AddEditorGUILayout(key, keyType);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        
        private void AddEditorGUILayout(string key, Type type)
        {
            if (type == typeof(float))
            {
                variables.floats[key] = EditorGUILayout.FloatField(variables.floats[key]);
            }
            else if (type == typeof(Color))
            {
                variables.colors[key] = EditorGUILayout.ColorField(variables.colors[key]);
            }
            else if (type == typeof(Vector2))
            {
                variables.vector2s[key] = EditorGUILayout.Vector2Field("", variables.vector2s[key]);
            }
            else if (type == typeof(Vector3))
            {
                variables.vector3s[key] = EditorGUILayout.Vector3Field("", variables.vector3s[key]);
            }
            else if (type == typeof(Sprite))
            {
                variables.sprites[key] = (Sprite)EditorGUILayout.ObjectField(variables.sprites[key], typeof(Sprite), true);
            }
            else if (type == typeof(bool))
            {
                variables.bools[key] = EditorGUILayout.Toggle(variables.bools[key]);
            }
            else
            {
                throw new Exception("Type is not implemented.");
            }
        }
        private void GetKeys()
        {
            keys = new string[variables.labels[selectedLabel].keys.Count];
            variables.labels[selectedLabel].keys.CopyTo(keys);
        }
        private void UpdateKeyInVariableEvents(string oldKey, string newKey)
        {
            activeScene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.SaveOpenScenes();
            sceneCount = EditorSceneManager.sceneCountInBuildSettings;
            scenePaths = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenePaths[i] = SceneUtility.GetScenePathByBuildIndex(i);
                if (i == activeScene.buildIndex)
                    activeScenePath = scenePaths[i];
            }


            foreach (string sceneName in scenePaths)
            {
                EditorSceneManager.OpenScene(sceneName);
                scene = EditorSceneManager.GetActiveScene();

                allGameObjects = FindObjectsOfType<GameObject>(true);
                derivedComponents = new List<Component>();

                foreach (GameObject obj in allGameObjects)
                {
                    components = obj.GetComponents<Component>();

                    foreach (var component in components)
                    {
                        if (component == null) continue;
                        componentType = component.GetType();

                        if (IsDerivedFromGenericType(componentType, typeof(EditorVariableEvent<>)))
                        {
                            derivedComponents.Add(component);
                        }
                    }
                }

                foreach (Component comp in derivedComponents)
                {
                    componentType = comp.GetType();
                    keyField = componentType.GetField("key");
                    if (keyField == null) return;

                    if ((string)keyField.GetValue(comp) == oldKey)
                    {
                        keyField.SetValue(comp, newKey);
                    }
                }

                EditorSceneManager.SaveScene(scene);
            }
            EditorSceneManager.OpenScene(activeScenePath);
            
        }
        private static bool IsDerivedFromGenericType(Type type, Type genericType)
        {
            while (type != null && type != typeof(object))
            {
                Type currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (genericType == currentType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }
        private static MethodInfo GetInvokeMethod(Type objectType, Type genericArg)
        {
            MethodInfo[] methods = objectType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (method.Name == "Invoke" && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == genericArg)
                {
                    return method;
                }
            }

            return null;
        }

    }
}

#endif
