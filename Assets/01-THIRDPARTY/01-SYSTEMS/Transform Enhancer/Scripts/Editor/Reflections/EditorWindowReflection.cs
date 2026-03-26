using System;
using System.Reflection;
using UnityEditor;

namespace TransformEnhancer.Editor
{
    public class EditorWindowReflection
    {
        private static Type m_ClassType = typeof(EditorWindow);

        public static Type ClassType => m_ClassType;

        private static FieldInfo s_ParentField;

        public static FieldInfo ParentField
        {
            get
            {
                if (s_ParentField == null)
                {
                    s_ParentField = ClassType.GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (s_ParentField == null)
                {
                    throw new NullReferenceException("The EditorWindow.m_Parent is not exist.");
                }
                return s_ParentField;
            }
        }

        // HostView
        public static object GetParent(EditorWindow instance)
        {
            return ParentField.GetValue(instance);
        }
    }
}