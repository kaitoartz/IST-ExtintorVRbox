using System;
using System.Reflection;

namespace TransformEnhancer.Editor
{
    public class LocalizationDatabaseReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.LocalizationDatabase");
                }
                return s_ClassType;
            }
        }

        private static MethodInfo s_GetLocalizedStringMethod;

        public static MethodInfo GetLocalizedStringMethod
        {
            get
            {
                if (s_GetLocalizedStringMethod == null)
                {
                    s_GetLocalizedStringMethod = ClassType.GetMethod("GetLocalizedString", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
                }
                if (s_GetLocalizedStringMethod == null)
                {
                    throw new NullReferenceException("The LocalizationDatabase.GetLocalizedString is not exist.");
                }
                return s_GetLocalizedStringMethod;
            }
        }

        public static string GetLocalizedString(string original)
        {
            return GetLocalizedStringMethod.Invoke(null, new object[] { original }) as string;
        }
    }
}