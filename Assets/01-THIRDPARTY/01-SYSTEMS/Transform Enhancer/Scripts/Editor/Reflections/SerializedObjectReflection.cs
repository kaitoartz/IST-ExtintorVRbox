using System;
using System.Reflection;
using UnityEditor;

namespace TransformEnhancer.Editor
{
    public class SerializedObjectReflection
    {
        private static Type m_ClassType = typeof(SerializedObject);

        public static Type ClassType => m_ClassType;

        private static PropertyInfo s_TargetObjectsCountProperty;

        public static PropertyInfo TargetObjectsCountProperty
        {
            get
            {
                if (s_TargetObjectsCountProperty == null)
                {
                    s_TargetObjectsCountProperty = ClassType.GetProperty("targetObjectsCount", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (s_TargetObjectsCountProperty == null)
                {
                    throw new NullReferenceException("The Editor.targetObjectsCount is not exist.");
                }
                return s_TargetObjectsCountProperty;
            }
        }

        public static int GetTargetObjectsCount(SerializedObject instance)
        {
            return (int)TargetObjectsCountProperty.GetValue(instance, null);
        }
    }
}