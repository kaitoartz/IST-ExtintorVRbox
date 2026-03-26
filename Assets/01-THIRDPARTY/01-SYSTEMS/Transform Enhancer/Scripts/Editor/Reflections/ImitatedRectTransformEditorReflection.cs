using System;
using System.Reflection;

namespace TransformEnhancer.Editor
{
    public class ImitatedRectTransformEditorReflection
    {
        private static Type s_ClassType = typeof(ImitatedRectTransformEditor);

        public static Type ClassType => s_ClassType;

        private static MethodInfo s_HandleDragChangeMethod;

        public static MethodInfo HandleDragChangeMethod
        {
            get
            {
                if (s_HandleDragChangeMethod == null)
                {
                    s_HandleDragChangeMethod = ClassType.GetMethod("HandleDragChange", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), typeof(bool) }, null);
                }
                if (s_HandleDragChangeMethod == null)
                {
                    throw new NullReferenceException("The ImitatedRectTransformEditor.HandleDragChange is not exist.");
                }
                return s_HandleDragChangeMethod;
            }
        }
    }
}