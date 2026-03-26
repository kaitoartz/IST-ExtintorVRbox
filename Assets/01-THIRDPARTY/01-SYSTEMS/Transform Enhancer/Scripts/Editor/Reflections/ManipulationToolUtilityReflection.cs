using System;
using System.Reflection;

namespace TransformEnhancer.Editor
{
    public class ManipulationToolUtilityReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.ManipulationToolUtility");
                }
                return s_ClassType;
            }
        }

        private static FieldInfo s_HandleDragChangeField;

        public static FieldInfo HandleDragChangeField
        {
            get
            {
                if (s_HandleDragChangeField == null)
                {
                    s_HandleDragChangeField = ClassType.GetField("handleDragChange", BindingFlags.Static | BindingFlags.Public);
                }
                if (s_HandleDragChangeField == null)
                {
                    throw new NullReferenceException("The ManipulationToolUtility.handleDragChange is not exist.");
                }
                return s_HandleDragChangeField;
            }
        }

        public static void AddHandleDragChange(object instance, MethodInfo method)
        {
            var handler = Delegate.CreateDelegate(HandleDragChangeField.FieldType, instance, method);
            var curDelegate = (Delegate)HandleDragChangeField.GetValue(null);
            var newDelegate = Delegate.Combine(curDelegate, handler);
            HandleDragChangeField.SetValue(null, newDelegate);
        }

        public static void SubHandleDragChange(object instance, MethodInfo method)
        {
            var handler = Delegate.CreateDelegate(HandleDragChangeField.FieldType, instance, method);
            var curDelegate = (Delegate)HandleDragChangeField.GetValue(null);
            var newDelegate = Delegate.Remove(curDelegate, handler);
            HandleDragChangeField.SetValue(null, newDelegate);
        }
    }
}