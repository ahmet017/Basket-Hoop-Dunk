using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityUtils.Editor
{

    public class FixNaNValues : MonoBehaviour
    {
        [MenuItem("GameObject/UnityUtils/Tools/Fix NaN Values/Rect Transform")]
        public static void FixNaNValuesOfRectTransforms()
        {
            Debug.Log("Fixing rect transforms");
            EditorSceneManager.MarkAllScenesDirty();

            RectTransform[] rects = GameObject.FindObjectsOfType<RectTransform>(true);
            foreach (RectTransform rect in rects)
            {
                if (float.IsNaN(rect.anchoredPosition.x) || float.IsNaN(rect.anchoredPosition.y))
                {
                    rect.anchoredPosition = Vector2.zero;
                    Debug.Log(rect.gameObject.name + " is fixed");
                }
            }

            EditorSceneManager.SaveOpenScenes();
        }

        [MenuItem("GameObject/UnityUtils/Tools/Fix NaN Values/Text Mesh Pro")]
        public static void FixNaNValuesOfTexts()
        {
            Debug.Log("Fixing test objects");
            EditorSceneManager.MarkAllScenesDirty();

            TextMeshProUGUI[] texts = GameObject.FindObjectsOfType<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI text in texts)
            {
                if (float.IsNaN(text.margin.x) || float.IsNaN(text.margin.y) || float.IsNaN(text.margin.z) || float.IsNaN(text.margin.w))
                {
                    text.margin = Vector4.zero;
                    Debug.Log(text.gameObject.name + " is fixed");
                }
            }

            EditorSceneManager.SaveOpenScenes();
        }
    }
}

#endif