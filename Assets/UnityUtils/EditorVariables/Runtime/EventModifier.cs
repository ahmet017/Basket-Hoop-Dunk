using UnityEngine;

namespace UnityUtils.EditorVariables
{
    public abstract class EventModifier<T> 
    {
        public virtual T GetModifiedValue(T value) { throw new System.NotImplementedException(); }
    }

    [System.Serializable]
    public class ColorModifier : EventModifier<Color>
    {
        [Range(0f, 1f)] public float alpha;

        public override Color GetModifiedValue(Color color)
        {
            color.a = alpha;
            return color;
        }
    }

    [System.Serializable]
    public class FloatModifier : EventModifier<float>
    {
        public float add = 0;
        public float multiply = 1;

        public override float GetModifiedValue(float f)
        {
            f += add;
            f *= multiply;
            return f;
        }
    }

    [System.Serializable]
    public class Vector2Modifier : EventModifier<Vector2>
    {
        public Vector2 add = Vector2.zero;
        public float multiply = 1;

        public override Vector2 GetModifiedValue(Vector2 v)
        {
            v += add;
            v *= multiply;
            return v;
        }
    }

    [System.Serializable]
    public class Vector3Modifier : EventModifier<Vector3>
    {
        public Vector3 add = Vector3.zero;
        public float multiply = 1;

        public override Vector3 GetModifiedValue(Vector3 v)
        {
            v += add;
            v *= multiply;
            return v;
        }
    }
}
