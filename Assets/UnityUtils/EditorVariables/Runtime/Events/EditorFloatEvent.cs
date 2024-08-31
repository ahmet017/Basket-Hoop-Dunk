namespace UnityUtils.EditorVariables
{
    public class EditorFloatEvent : EditorVariableEvent<float>, IEditorVariableEvent<float>
    {
        public new FloatModifier modifier;

        public override void Invoke(float f)
        {
            base.Invoke(f);
            if (modifierEnabed)
                f = modifier.GetModifiedValue(f);
            Event.Invoke(f);
        }
    }
}

