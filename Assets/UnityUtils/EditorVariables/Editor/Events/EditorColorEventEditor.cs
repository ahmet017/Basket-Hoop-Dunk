using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorColorEvent))]
    public class EditorColorEventEditor : EditorVariableEventEditor<Color> { }
}
#endif
