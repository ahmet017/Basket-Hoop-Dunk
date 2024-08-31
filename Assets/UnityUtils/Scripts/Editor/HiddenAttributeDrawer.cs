using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.Editor
{
    [CustomPropertyDrawer(typeof(HiddenAttribute))]
    public class HiddenAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!HiddenGameObject.IsObjectsHidden)
                EditorGUI.PropertyField(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!HiddenGameObject.IsObjectsHidden)
                return base.GetPropertyHeight(property, label);
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
#endif
