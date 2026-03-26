using System;
using System.Reflection;

namespace TransformEnhancer.Editor
{
    public class PlayModeViewReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.PlayModeView");
                }
                return s_ClassType;
            }
        }

        private static MethodInfo s_RepaintAllMethod;

        public static MethodInfo RepaintAllMethod
        {
            get
            {
                if (s_RepaintAllMethod == null)
                {
                    s_RepaintAllMethod = ClassType.GetMethod("RepaintAll", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] {}, null);
                }
                if (s_RepaintAllMethod == null)
                {
                    throw new NullReferenceException("The PlayModeView.RepaintAll is not exist.");
                }
                return s_RepaintAllMethod;
            }
        }

        public static void RepaintAll()
        {
            RepaintAllMethod.Invoke(null, null);
        }
    }
}