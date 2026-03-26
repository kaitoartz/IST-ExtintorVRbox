using UnityEditor;

namespace TransformEnhancer.Editor
{
    public static class EditorWindowExtension
    {
        public static object GetParent(this EditorWindow instance)
        {
            return EditorWindowReflection.GetParent(instance);
        }
    }
}