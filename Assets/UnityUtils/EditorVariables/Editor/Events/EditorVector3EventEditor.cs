using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorVector3Event))]
    public class EditorVector3EventEditor : EditorVariableEventEditor<Vector3> { }
}
#endif
