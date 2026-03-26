using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    public class TransformRotationGUIReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.TransformRotationGUI");
                }
                return s_ClassType;
            }
        }

        private object m_Instance;

        public object Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = Activator.CreateInstance(ClassType);
                }
                return m_Instance;
            }
        }

        private static MethodInfo s_OnEnableMethod;

        public static MethodInfo OnEnableMethod
        {
            get
            {
                if (s_OnEnableMethod == null)
                {
                    s_OnEnableMethod = ClassType.GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(SerializedProperty), typeof(GUIContent) }, null);
                }
                if (s_OnEnableMethod == null)
                {
                    throw new NullReferenceException("The TransformRotationGUI.OnEnable is not exist.");
                }
                return s_OnEnableMethod;
            }
        }

        public void OnEnable(SerializedProperty rotation, GUIContent label)
        {
            OnEnableMethod.Invoke(Instance, new object[] { rotation, label });
        }

        private static MethodInfo s_RotationFieldMethod1;

        public static MethodInfo RotationFieldMethod1
        {
            get
            {
                if (s_RotationFieldMethod1 == null)
                {
                    s_RotationFieldMethod1 = ClassType.GetMethod("RotationField", BindingFlags.Instance | BindingFlags.Public, null, new Type[] {}, null);
                }
                if (s_RotationFieldMethod1 == null)
                {
                    throw new NullReferenceException("The TransformRotationGUI.RotationField is not exist.");
                }
                return s_RotationFieldMethod1;
            }
        }

        public void RotationField()
        {
            RotationFieldMethod1.Invoke(Instance, null);
        }

        private static MethodInfo s_RotationFieldMethod2;

        public static MethodInfo RotationFieldMethod2
        {
            get
            {
                if (s_RotationFieldMethod2 == null)
                {
                    s_RotationFieldMethod2 = ClassType.GetMethod("RotationField", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(bool) }, null);
                }
                if (s_RotationFieldMethod2 == null)
                {
                    throw new NullReferenceException("The TransformRotationGUI.RotationField is not exist.");
                }
                return s_RotationFieldMethod2;
            }
        }

        public void RotationField(bool disabled)
        {
            RotationFieldMethod2.Invoke(Instance, new object[] { disabled });
        }
    }
}