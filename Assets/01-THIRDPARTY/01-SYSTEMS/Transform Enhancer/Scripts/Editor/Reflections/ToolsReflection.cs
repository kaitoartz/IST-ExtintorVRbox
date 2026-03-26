using System;
using System.Reflection;
using UnityEditor;

namespace TransformEnhancer.Editor
{
    public class ToolsReflection
    {
        private static Type s_ClassType = typeof(Tools);

        public static Type ClassType => s_ClassType;

        private static MethodInfo s_RepaintAllToolViewsMethod;

        public static MethodInfo RepaintAllToolViewsMethod
        {
            get
            {
                if (s_RepaintAllToolViewsMethod == null)
                {
                    s_RepaintAllToolViewsMethod = ClassType.GetMethod("RepaintAllToolViews", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (s_RepaintAllToolViewsMethod == null)
                {
                    throw new NullReferenceException("The Tools.RepaintAllToolViews is not exist.");
                }
                return s_RepaintAllToolViewsMethod;
            }
        }

        public static void RepaintAllToolViews()
        {
            RepaintAllToolViewsMethod.Invoke(null, null);
        }
    }
}