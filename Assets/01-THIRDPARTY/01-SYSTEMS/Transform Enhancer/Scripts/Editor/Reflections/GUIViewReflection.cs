using System;
using System.Reflection;

namespace TransformEnhancer.Editor
{
    public class GUIViewReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.GUIView");
                }
                return s_ClassType;
            }
        }

        private static PropertyInfo s_CurrentProperty;

        public static PropertyInfo CurrentProperty
        {
            get
            {
                if (s_CurrentProperty == null)
                {
                    s_CurrentProperty = ClassType.GetProperty("current", BindingFlags.Static | BindingFlags.Public);
                }
                if (s_CurrentProperty == null)
                {
                    throw new NullReferenceException("The GUIView.current is not exist.");
                }
                return s_CurrentProperty;
            }
        }

        // GUIView
        public static object Current => CurrentProperty.GetValue(null);
    }
}