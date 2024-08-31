using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.EditorVariables.Editor
{
    [CustomEditor(typeof(EditorSpriteEvent))]
    public class EditorSpriteEventEditor : EditorVariableEventEditor<Sprite> { }
}
#endif