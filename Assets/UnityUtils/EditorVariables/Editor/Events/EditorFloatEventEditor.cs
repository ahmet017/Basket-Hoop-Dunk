

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorFloatEvent))]
    public class EditorFloatEventEditor : EditorVariableEventEditor<float> { }

}
#endif
