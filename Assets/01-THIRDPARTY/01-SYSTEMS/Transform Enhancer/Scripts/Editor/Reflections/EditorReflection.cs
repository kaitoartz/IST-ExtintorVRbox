using System;
using System.Reflection;

namespace TransformEnhancer.Editor
{
    public class EditorReflection
    {
        private static Type m_ClassType = typeof(UnityEditor.Editor);

        public static Type ClassType => m_ClassType;

        private static MethodInfo s_OnForceReloadInspectorMethod;

        public static MethodInfo OnForceReloadInspectorMethod
        {
            get
            {
                if (s_OnForceReloadInspectorMethod == null)
                {
                    s_OnForceReloadInspectorMethod = ClassType.GetMethod("OnForceReloadInspector", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (s_OnForceReloadInspectorMethod == null)
                {
                    throw new NullReferenceException("The Editor.OnForceReloadInspector is not exist.");
                }
                return s_OnForceReloadInspectorMethod;
            }
        }

        public static void OnForceReloadInspector(UnityEditor.Editor instance)
        {
            OnForceReloadInspectorMethod.Invoke(instance, null);
        }
    }
}