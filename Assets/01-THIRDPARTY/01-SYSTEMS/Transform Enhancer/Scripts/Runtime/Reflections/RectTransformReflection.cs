using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TransformEnhancer.Runtime
{
    public class RectTransformReflection
    {
        private static Type s_ClassType = typeof(RectTransform);

        public static Type ClassType => s_ClassType;

        private static PropertyInfo s_DrivenByObjectProperty;

        public static PropertyInfo DrivenByObjectProperty
        {
            get
            {
                if (s_DrivenByObjectProperty == null)
                {
                    s_DrivenByObjectProperty = ClassType.GetProperty("drivenByObject", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                if (s_DrivenByObjectProperty == null)
                {
                    throw new NullReferenceException("The RectTransform.drivenByObject is not exist.");
                }
                return s_DrivenByObjectProperty;
            }
        }

        public static Object GetDrivenByObject(RectTransform instance)
        {
            return (Object)DrivenByObjectProperty.GetValue(instance);
        }

        private static PropertyInfo s_DrivenPropertiesProperty;

        public static PropertyInfo DrivenPropertiesProperty
        {
            get
            {
                if (s_DrivenPropertiesProperty == null)
                {
                    s_DrivenPropertiesProperty = ClassType.GetProperty("drivenProperties", BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (s_DrivenPropertiesProperty == null)
                {
                    throw new NullReferenceException("The RectTransform.drivenProperties is not exist.");
                }
                return s_DrivenPropertiesProperty;
            }
        }

        public static DrivenTransformProperties GetDrivenProperties(RectTransform instance)
        {
            return (DrivenTransformProperties)DrivenPropertiesProperty.GetValue(instance);
        }

        private static MethodInfo s_SendReapplyDrivenPropertiesMethod;

        public static MethodInfo SendReapplyDrivenPropertiesMethod
        {
            get
            {
                if (s_SendReapplyDrivenPropertiesMethod == null)
                {
                    s_SendReapplyDrivenPropertiesMethod = ClassType.GetMethod("SendReapplyDrivenProperties", BindingFlags.Static | BindingFlags.NonPublic);
                }
                if (s_SendReapplyDrivenPropertiesMethod == null)
                {
                    throw new NullReferenceException("The RectTransform.SendReapplyDrivenProperties is not exist.");
                }
                return s_SendReapplyDrivenPropertiesMethod;
            }
        }

        public static void SendReapplyDrivenProperties(RectTransform driven)
        {
            SendReapplyDrivenPropertiesMethod.Invoke(null, new object[] { driven });
        }

        private static MethodInfo s_GetRectInParentSpaceMethod;

        public static MethodInfo GetRectInParentSpaceMethod
        {
            get
            {
                if (s_GetRectInParentSpaceMethod == null)
                {
                    s_GetRectInParentSpaceMethod = ClassType.GetMethod("GetRectInParentSpace", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (s_GetRectInParentSpaceMethod == null)
                {
                    throw new NullReferenceException("The RectTransform.GetRectInParentSpace is not exist.");
                }
                return s_GetRectInParentSpaceMethod;
            }
        }

        public static Rect GetRectInParentSpace(RectTransform instance)
        {
            return (Rect)GetRectInParentSpaceMethod.Invoke(instance, null);
        }
    }
}