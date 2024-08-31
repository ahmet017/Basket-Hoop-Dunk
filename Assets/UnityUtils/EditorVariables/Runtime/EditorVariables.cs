using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace UnityUtils.EditorVariables
{
    public class EditorVariables : ScriptableObject
    {
        public static readonly string DEFAULT_LABEL = "Default Label";

        public SerializableDictionary<string, float> floats = new SerializableDictionary<string, float>();
        public SerializableDictionary<string, Color> colors = new SerializableDictionary<string, Color>();
        public SerializableDictionary<string, Vector2> vector2s = new SerializableDictionary<string, Vector2>();
        public SerializableDictionary<string, Vector3> vector3s = new SerializableDictionary<string, Vector3>();
        public SerializableDictionary<string, Sprite> sprites = new SerializableDictionary<string, Sprite>();
        public SerializableDictionary<string, bool> bools = new SerializableDictionary<string, bool>();
        public SerializableDictionary<string, Label> labels = new SerializableDictionary<string, Label>();

        private FieldInfo[] fieldInfos;
        private Type fieldType;
        private Type[] genericTypes;
        private MethodInfo methodInfo;
        private object fieldValue;

        public T GetValue<T>(string key)
        {
            try
            {
                T result = default(T);
                fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo info in fieldInfos)
                {
                    fieldValue = info.GetValue(this);
                    if (fieldValue is SerializableDictionary<string, T>)
                    {
                        result = (T)Convert.ChangeType(((SerializableDictionary<string, T>)fieldValue)[key], typeof(T));
                    }
                }
                return result;
            }
            catch
            {
                return default;
            }
        }
        public void AddValue(string key, string label, object value, Type type)
        {
            fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fieldInfos)
            {
                fieldType = info.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    genericTypes = fieldType.GetGenericArguments();
                    if (genericTypes[1] == type)
                    {
                        methodInfo = fieldType.GetMethod("Add", new[] { typeof(string), type });
                        fieldValue = info.GetValue(this);
                        methodInfo.Invoke(fieldValue, new object[] { key, Convert.ChangeType(value, type) });
                    }
                }
            }
            labels[label].keys.Add(key);
        }
        public void CopyKeys<T>(out string[] keys)
        {
            keys = null;
            fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fieldInfos)
            {
                fieldType = info.FieldType;
                fieldValue = info.GetValue(this);
                if (fieldValue is SerializableDictionary<string, T>)
                {
                    keys = new string[((SerializableDictionary<string, T>)fieldValue).Count];
                    ((SerializableDictionary<string, T>)fieldValue).Keys.CopyTo(keys, 0);
                }
            }
        }
        public bool ContainsKey(string key)
        {
            fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fieldInfos)
            {
                fieldType = info.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    methodInfo = fieldType.GetMethod("ContainsKey", new[] { typeof(string) }); 
                    fieldValue = info.GetValue(this);
                    if ((bool)methodInfo.Invoke(fieldValue, new string[] { key }))
                        return true;
                }
            }
            return false;
        }
        public void RemoveKey(string key)
        {
            foreach (var label in labels)
            {
                if (label.Value.keys.Contains(key))
                {
                    label.Value.keys.Remove(key);
                    break;
                }
            }

            fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fieldInfos)
            {
                fieldType = info.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    methodInfo = fieldType.GetMethod("Remove", new[] {typeof(string)});
                    fieldValue = info.GetValue(this);
                    if ((bool)methodInfo.Invoke(fieldValue, new string[] { key }))
                        return;
                }
            }
        }
        public void UpdateVariable(string oldKey, string newKey, string newLabel)
        {
            foreach (var label in labels)
            {
                if (label.Value.keys.Contains(oldKey))
                {
                    label.Value.keys.Remove(oldKey);
                    labels[newLabel].keys.Add(newKey);
                    break;
                }
            }

            fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fieldInfos)
            {
                fieldType = info.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    methodInfo = fieldType.GetMethod("ContainsKey", new[] { typeof(string) });
                    fieldValue = info.GetValue(this);
                    if ((bool)methodInfo.Invoke(fieldValue, new string[] { oldKey }))
                    {
                        genericTypes = fieldType.GetGenericArguments();
                        object value = ((IDictionary)fieldValue)[oldKey];
                        ((IDictionary)fieldValue).Remove(oldKey);
                        ((IDictionary)fieldValue).Add(newKey, Convert.ChangeType(value, genericTypes[1]));
                        return;
                    }
                }
            }
        }
        public Type GetTypeOfTheKey(string key)
        {
            fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fieldInfos)
            {
                fieldType = info.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    methodInfo = fieldType.GetMethod("ContainsKey", new[] { typeof(string) });
                    fieldValue = info.GetValue(this);
                    if ((bool)methodInfo.Invoke(fieldValue, new string[] { key }))
                    {
                        genericTypes = fieldType.GetGenericArguments();
                        return genericTypes[1];
                    }
                }
            }
            throw new Exception("Key is not contained");
        }
        public void CopyLabels(out string[] labelNames)
        {
            labelNames = new string[this.labels.Count];
            this.labels.Keys.CopyTo(labelNames, 0);
        }
        public void RemoveLabel(string label)
        {
            foreach (string key in labels[label].keys)
            {
                labels[DEFAULT_LABEL].keys.Add(key);
            }
            labels.Remove(label);
        }
        public void ReplaceKeys(string label, string key1, string key2)
        {
            int indexKey1 = labels[label].keys.IndexOf(key1);
            int indexKey2 = labels[label].keys.IndexOf(key2);
            labels[label].keys[indexKey1] = key2;
            labels[label].keys[indexKey2] = key1;
        }

        public static List<Type> GetAvailableTypes()
        {
            List<Type> availableTypes = new List<Type>();

            FieldInfo[] fieldInfos = typeof(EditorVariables).GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fieldInfos)
            {
                Type fieldType = info.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    Type[] genericTypes = fieldType.GetGenericArguments();
                    if (genericTypes[1] != typeof(Label))
                    {
                        availableTypes.Add(genericTypes[1]);
                    }
                }
            }

            return availableTypes;
        }
    }
}

