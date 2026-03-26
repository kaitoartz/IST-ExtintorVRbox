using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace TransformEnhancer.Editor
{
    public class SceneViewReflection
    {
        private static Type s_ClassType = typeof(SceneView);

        public static Type ClassType => s_ClassType;

        private static PropertyInfo s_ActiveEditorsProperty;

        public static PropertyInfo ActiveEditorsProperty
        {
            get
            {
                if (s_ActiveEditorsProperty == null)
                {
                    s_ActiveEditorsProperty = ClassType.GetProperty("activeEditors", BindingFlags.Static | BindingFlags.NonPublic);
                }
                if (s_ActiveEditorsProperty == null)
                {
                    throw new NullReferenceException("The SceneView.activeEditors is not exist.");
                }
                return s_ActiveEditorsProperty;
            }
        }

        public static IEnumerable<UnityEditor.Editor> ActiveEditors => (IEnumerable<UnityEditor.Editor>)ActiveEditorsProperty.GetValue(null, null);
    }
}