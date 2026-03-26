using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace TransformEnhancer.Editor
{
    public class EditorGUIUtilityReflection
    {
        private static Type s_ClassType = typeof(EditorGUIUtility);

        public static Type ClassType => s_ClassType;

        private static FieldInfo s_LastControlIDField;

        public static FieldInfo LastControlIDField
        {
            get
            {
                if (s_LastControlIDField == null)
                {
                    s_LastControlIDField = ClassType.GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
                }
                if (s_LastControlIDField == null)
                {
                    throw new NullReferenceException("The EditorGUIUtility.s_LastControlID is not exist.");
                }
                return s_LastControlIDField;
            }
        }

        public static int LastControlID => (int)LastControlIDField.GetValue(null);

        private static MethodInfo s_IsGizmosAllowedForObjectMethod;

        public static MethodInfo IsGizmosAllowedForObjectMethod
        {
            get
            {
                if (s_IsGizmosAllowedForObjectMethod == null)
                {
                    s_IsGizmosAllowedForObjectMethod = ClassType.GetMethod("IsGizmosAllowedForObject", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Object) }, null);
                }
                if (s_IsGizmosAllowedForObjectMethod == null)
                {
                    throw new NullReferenceException("The EditorGUIUtility.IsGizmosAllowedForObject is not exist.");
                }
                return s_IsGizmosAllowedForObjectMethod;
            }
        }

        public static bool IsGizmosAllowedForObject(Object obj)
        {
            return (bool)IsGizmosAllowedForObjectMethod.Invoke(null, new object[] { obj });
        }
    }
}