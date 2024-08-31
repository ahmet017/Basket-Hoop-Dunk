
namespace UnityUtils.EditorVariables
{
    public interface IEditorVariableEvent<T>
    {
        void Invoke(T value);
    }
}