using System;
using System.Reflection;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    public class RectTransformSnappingReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.RectTransformSnapping");
                }
                return s_ClassType;
            }
        }

        private static FieldInfo s_SnapThresholdField;

        public static FieldInfo SnapThresholdField
        {
            get
            {
                if (s_SnapThresholdField == null)
                {
                    s_SnapThresholdField = ClassType.GetField("kSnapThreshold", BindingFlags.Static | BindingFlags.NonPublic);
                }
                if (s_SnapThresholdField == null)
                {
                    throw new NullReferenceException("The RectTransformSnapping.kSnapThreshold is not exist.");
                }
                return s_SnapThresholdField;
            }
        }

        public static float SnapThreshold => (float)SnapThresholdField.GetValue(null);

        private static MethodInfo s_OnGUIMethod;

        public static MethodInfo OnGUIMethod
        {
            get
            {
                if (s_OnGUIMethod == null)
                {
                    s_OnGUIMethod = ClassType.GetMethod("OnGUI", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] {}, null);
                }
                if (s_OnGUIMethod == null)
                {
                    throw new NullReferenceException("The RectTransformSnapping.OnGUI is not exist.");
                }
                return s_OnGUIMethod;
            }
        }

        public static void OnGUI()
        {
            OnGUIMethod.Invoke(null, null);
        }

        private static MethodInfo s_DrawGuidesMethod;

        public static MethodInfo DrawGuidesMethod
        {
            get
            {
                if (s_DrawGuidesMethod == null)
                {
                    s_DrawGuidesMethod = ClassType.GetMethod("DrawGuides", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] {}, null);
                }
                if (s_DrawGuidesMethod == null)
                {
                    throw new NullReferenceException("The RectTransformSnapping.DrawGuides is not exist.");
                }
                return s_DrawGuidesMethod;
            }
        }

        public static void DrawGuides()
        {
            DrawGuidesMethod.Invoke(null, null);
        }

        private static MethodInfo s_CalculateAnchorSnapValuesMethod;

        public static MethodInfo CalculateAnchorSnapValuesMethod
        {
            get
            {
                if (s_CalculateAnchorSnapValuesMethod == null)
                {
                    s_CalculateAnchorSnapValuesMethod = ClassType.GetMethod("CalculateAnchorSnapValues", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(Transform),
                        typeof(Transform),
                        typeof(RectTransform),
                        typeof(int),
                        typeof(int)
                    }, null);
                }
                if (s_CalculateAnchorSnapValuesMethod == null)
                {
                    throw new NullReferenceException("The RectTransformSnapping.CalculateAnchorSnapValues is not exist.");
                }
                return s_CalculateAnchorSnapValuesMethod;
            }
        }

        public static void CalculateAnchorSnapValues(Transform parentSpace,
                                                     Transform self,
                                                     RectTransform gui,
                                                     int minmaxX,
                                                     int minmaxY)
        {
            CalculateAnchorSnapValuesMethod.Invoke(null, new object[] { parentSpace, self, gui, minmaxX, minmaxY });
        }

        private static MethodInfo s_SnapToGuidesMethod;

        public static MethodInfo SnapToGuidesMethod
        {
            get
            {
                if (s_SnapToGuidesMethod == null)
                {
                    s_SnapToGuidesMethod = ClassType.GetMethod("SnapToGuides", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(float),
                        typeof(float),
                        typeof(int)
                    }, null);
                }
                if (s_SnapToGuidesMethod == null)
                {
                    throw new NullReferenceException("The RectTransformSnapping.SnapToGuides is not exist.");
                }
                return s_SnapToGuidesMethod;
            }
        }

        public static float SnapToGuides(float value, float snapDistance, int axis)
        {
            return (float)SnapToGuidesMethod.Invoke(null, new object[] { value, snapDistance, axis });
        }
    }
}