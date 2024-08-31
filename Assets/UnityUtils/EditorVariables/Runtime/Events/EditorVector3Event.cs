using UnityEngine;

namespace UnityUtils.EditorVariables
{
    public class EditorVector3Event : EditorVariableEvent<Vector3>, IEditorVariableEvent<Vector3>
    {
        public new Vector3Modifier modifier;

        public override void Invoke(Vector3 v)
        {
            base.Invoke(v);
            if (modifierEnabed)
                v = modifier.GetModifiedValue(v);
            Event.Invoke(v);
        }
    }
}

