namespace TransformEnhancer.Editor
{
    public static class EditorExtension
    {
        public static void OnForceReloadInspector(this UnityEditor.Editor instance)
        {
            EditorReflection.OnForceReloadInspector(instance);
        }
    }
}