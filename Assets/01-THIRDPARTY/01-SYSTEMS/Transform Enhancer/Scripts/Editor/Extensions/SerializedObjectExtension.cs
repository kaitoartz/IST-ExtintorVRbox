using UnityEditor;

namespace TransformEnhancer.Editor
{
    public static class SerializedObjectExtension
    {
        public static int GetTargetObjectsCount(this SerializedObject serializedObject)
        {
            return SerializedObjectReflection.GetTargetObjectsCount(serializedObject);
        }
    }
}