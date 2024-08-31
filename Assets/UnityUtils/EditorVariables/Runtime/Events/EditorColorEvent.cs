using UnityEngine;

namespace UnityUtils.EditorVariables
{
    public class EditorColorEvent : EditorVariableEvent<Color>, IEditorVariableEvent<Color>
    {
        public new ColorModifier modifier;

        public override void Invoke(Color color)
        {
            base.Invoke(color);
            if (modifierEnabed)
                color = modifier.GetModifiedValue(color);
            Event.Invoke(color);
        }
    }
}

