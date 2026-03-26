using System;
using System.Reflection;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    public class GUIContentReflection
    {
        private static Type s_ClassType = typeof(GUIContent);

        public static Type ClassType => s_ClassType;

        private static MethodInfo s_TempMethod;

        public static MethodInfo TempMethod
        {
            get
            {
                if (s_TempMethod == null)
                {
                    s_TempMethod = ClassType.GetMethod("Temp", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
                }
                if (s_TempMethod == null)
                {
                    throw new NullReferenceException("The GUIContent.Temp is not exist.");
                }
                return s_TempMethod;
            }
        }

        public static GUIContent Temp(string t)
        {
            return (GUIContent)TempMethod.Invoke(null, new object[] { t });
        }
    }
}