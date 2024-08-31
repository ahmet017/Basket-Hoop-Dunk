using UnityEngine;

namespace UnityUtils.EditorVariables
{
    public class EditorVector2Event : EditorVariableEvent<Vector2>, IEditorVariableEvent<Vector2>
    {
        public new Vector2Modifier modifier;

        public override void Invoke(Vector2 v)
        {
            base.Invoke(v);
            if (modifierEnabed)
                v = modifier.GetModifiedValue(v);
            Event.Invoke(v);
        }
    }

}

