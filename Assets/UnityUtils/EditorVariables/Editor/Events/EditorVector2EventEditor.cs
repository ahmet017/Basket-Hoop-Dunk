using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorVector2Event))]
    public class EditorVector2EventEditor : EditorVariableEventEditor<Vector2> { }
}
#endif
