using System.Reflection;

namespace TransformEnhancer.Editor
{
    public class UnityEditorAssembly
    {
        public static readonly Assembly UnityEditor = Assembly.Load("UnityEditor");
    }
}