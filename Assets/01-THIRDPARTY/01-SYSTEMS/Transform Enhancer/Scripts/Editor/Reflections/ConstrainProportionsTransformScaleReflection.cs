using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TransformEnhancer.Editor
{
    public class ConstrainProportionsTransformScaleReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.ConstrainProportionsTransformScale");
                }
                return s_ClassType;
            }
        }

        private object m_Instance;

        public object Instance => m_Instance;

        private static PropertyInfo s_ConstrainProportionsScaleProperty;

        public static PropertyInfo ConstrainProportionsScaleProperty
        {
            get
            {
                if (s_ConstrainProportionsScaleProperty == null)
                {
                    s_ConstrainProportionsScaleProperty = ClassType.GetProperty("constrainProportionsScale", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (s_ConstrainProportionsScaleProperty == null)
                {
                    throw new NullReferenceException("The ConstrainProportionsTransformScale.constrainProportionsScale is not exist.");
                }
                return s_ConstrainProportionsScaleProperty;
            }
        }

        public bool ConstrainProportionsScale => (bool)ConstrainProportionsScaleProperty.GetValue(Instance, null);

        private static ConstructorInfo s_Constructor;

        public static ConstructorInfo Constructor
        {
            get
            {
                if (s_Constructor == null)
                {
                    s_Constructor = ClassType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Vector3) }, null);
                }
                if (s_Constructor == null)
                {
                    throw new NullReferenceException("The ConstrainProportionsTransformScale constructor is not exist.");
                }
                return s_Constructor;
            }
        }

        public ConstrainProportionsTransformScaleReflection(Vector3 previousScale)
        {
            // m_Instance = Activator.CreateInstance(ClassType, previousScale);
            m_Instance = Constructor.Invoke(new object[] { previousScale });
        }

        private static MethodInfo s_DoGUIMethod;

        public static MethodInfo DoGUIMethod
        {
            get
            {
                if (s_DoGUIMethod == null)
                {
                    s_DoGUIMethod = ClassType.GetMethod("DoGUI", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(Rect),
                        typeof(GUIContent),
                        typeof(Vector3),
                        typeof(Object[]),
                        typeof(int).MakeByRefType(),
                        typeof(SerializedProperty),
                        typeof(SerializedProperty)
                    }, null);
                }
                if (s_DoGUIMethod == null)
                {
                    throw new NullReferenceException("The ConstrainProportionsTransformScale.DoGUI is not exist.");
                }
                return s_DoGUIMethod;
            }
        }

        public Vector3 DoGUI(Rect rect,
                             GUIContent scaleContent,
                             Vector3 value,
                             Object[] targetObjects,
                             ref int axisModified,
                             SerializedProperty property = null,
                             SerializedProperty constrainProportionsProperty = null)
        {
            var parameters = new object[] { rect, scaleContent, value, targetObjects, axisModified, property, constrainProportionsProperty };
            var result = DoGUIMethod.Invoke(Instance, parameters);
            axisModified = (int)parameters[4];
            return (Vector3)result;
        }

        private static MethodInfo s_HandleMultiSelectionScaleChangesMethod;

        public static MethodInfo HandleMultiSelectionScaleChangesMethod
        {
            get
            {
                if (s_HandleMultiSelectionScaleChangesMethod == null)
                {
                    s_HandleMultiSelectionScaleChangesMethod = ClassType.GetMethod("HandleMultiSelectionScaleChanges", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(Vector3),
                        typeof(Vector3),
                        typeof(bool),
                        typeof(Object[]),
                        typeof(int).MakeByRefType()
                    }, null);
                }
                if (s_HandleMultiSelectionScaleChangesMethod == null)
                {
                    throw new NullReferenceException("The ConstrainProportionsTransformScale.HandleMultiSelectionScaleChanges is not exist.");
                }
                return s_HandleMultiSelectionScaleChangesMethod;
            }
        }

        public static bool HandleMultiSelectionScaleChanges(Vector3 scale,
                                                            Vector3 currentScale,
                                                            bool constrainProportionsScale,
                                                            Object[] targetObjects,
                                                            ref int axisModified)
        {
            var parameters = new object[] { scale, currentScale, constrainProportionsScale, targetObjects, axisModified };
            var result = HandleMultiSelectionScaleChangesMethod.Invoke(null, parameters);
            axisModified = (int)parameters[4];
            return (bool)result;
        }

        private static MethodInfo s_GetMixedValueFieldsMethod;

        public static MethodInfo GetMixedValueFieldsMethod
        {
            get
            {
                if (s_GetMixedValueFieldsMethod == null)
                {
                    s_GetMixedValueFieldsMethod = ClassType.GetMethod("GetMixedValueFields", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(SerializedProperty) }, null);
                }
                if (s_GetMixedValueFieldsMethod == null)
                {
                    throw new NullReferenceException("The ConstrainProportionsTransformScale.GetMixedValueFields is not exist.");
                }
                return s_GetMixedValueFieldsMethod;
            }
        }

        public static uint GetMixedValueFields(SerializedProperty property)
        {
            return (uint)GetMixedValueFieldsMethod.Invoke(null, new object[] { property });
        }

        private static MethodInfo s_InitializeMethod;

        public static MethodInfo InitializeMethod
        {
            get
            {
                if (s_InitializeMethod == null)
                {
                    s_InitializeMethod = ClassType.GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Object[]) }, null);
                }
                if (s_InitializeMethod == null)
                {
                    throw new NullReferenceException("The ConstrainProportionsTransformScale.Initialize is not exist.");
                }
                return s_InitializeMethod;
            }
        }

        public bool Initialize(Object[] targetObjects)
        {
            return (bool)InitializeMethod.Invoke(Instance, new object[] { targetObjects });
        }

        private static MethodInfo s_IsBitMethod;

        public static MethodInfo IsBitMethod
        {
            get
            {
                if (s_IsBitMethod == null)
                {
                    s_IsBitMethod = ClassType.GetMethod("IsBit", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(uint), typeof(int) }, null);
                }
                if (s_IsBitMethod == null)
                {
                    throw new NullReferenceException("The ConstrainProportionsTransformScale.IsBit is not exist.");
                }
                return s_IsBitMethod;
            }
        }

        public static bool IsBit(uint mask, int index)
        {
            return (bool)IsBitMethod.Invoke(null, new object[] { mask, index });
        }
    }
}