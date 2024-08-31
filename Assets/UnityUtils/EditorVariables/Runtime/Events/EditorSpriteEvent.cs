using UnityEngine;

namespace UnityUtils.EditorVariables
{
    public class EditorSpriteEvent : EditorVariableEvent<Sprite>, IEditorVariableEvent<Sprite>
    {
        public override void Invoke(Sprite sprite)
        {
            base.Invoke(sprite);
            Event.Invoke(sprite);
        }
    }
}