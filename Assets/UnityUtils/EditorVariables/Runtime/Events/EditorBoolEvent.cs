using UnityEngine;

namespace UnityUtils.EditorVariables
{
    public class EditorBoolEvent : EditorVariableEvent<bool>, IEditorVariableEvent<bool>
    {
        public override void Invoke(bool b)
        {
            base.Invoke(b);
            Event.Invoke(b);
        }
    }
}

