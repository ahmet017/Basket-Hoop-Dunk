using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils
{
    public static class PlayerPrefsExtension
    {
        public static readonly string separator = ",;/?:";

        public static void SetIntArray(string key, List<int> list)
        {
            string arrayString = string.Join(separator, list);
            PlayerPrefs.SetString(key, arrayString);
        }
        public static List<int> GetIntArray(string key)
        {
            string arrayString = PlayerPrefs.GetString(key);
            string[] arrayValues = arrayString.Split(separator);

            List<int> list = new List<int>();

            for (int i = 0; i < arrayValues.Length; i++)
            {
                if (int.TryParse(arrayValues[i], out int value))
                {
                    list.Add(value);
                }
            }
            return list;
        }

        public static void SetStringArray(string key, List<string> list)
        {
            string serializedArray = string.Join(separator, list);
            PlayerPrefs.SetString(key, serializedArray);
        }
        public static List<string> GetStringArray(string key)
        {
            List<string> list = new List<string>();
            string serializedArray = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(serializedArray))
                return list;

            return new List<string>(serializedArray.Split(separator));
        }

        public static void SetBool(string key, bool b)
        {
            PlayerPrefs.SetInt(key, b ? 1 : 0);
        }
        public static bool GetBool(string key, bool defaultValue)
        {
            if (!PlayerPrefs.HasKey(key))
                return defaultValue;
            return PlayerPrefs.GetInt(key, 0) == 1;
        }
    }
}
