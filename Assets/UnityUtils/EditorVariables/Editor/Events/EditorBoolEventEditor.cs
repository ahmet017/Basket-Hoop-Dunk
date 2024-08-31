using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorBoolEvent))]
    public class EditorBoolEventEditor : EditorVariableEventEditor<bool> { }
}
#endif
