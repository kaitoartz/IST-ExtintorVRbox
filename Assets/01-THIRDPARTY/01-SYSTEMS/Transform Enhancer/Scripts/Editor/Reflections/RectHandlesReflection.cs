using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    public class RectHandlesReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.RectHandles");
                }
                return s_ClassType;
            }
        }

        private static MethodInfo s_SideSliderMethod;

        public static MethodInfo SideSliderMethod
        {
            get
            {
                if (s_SideSliderMethod == null)
                {
                    s_SideSliderMethod = ClassType.GetMethod("SideSlider", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(int),
                        typeof(Vector3),
                        typeof(Vector3),
                        typeof(Vector3),
                        typeof(float),
                        typeof(Handles.CapFunction),
                        typeof(Vector2),
                        typeof(float)
                    }, null);
                }
                if (s_SideSliderMethod == null)
                {
                    throw new NullReferenceException("The RectHandles.SideSlider is not exist.");
                }
                return s_SideSliderMethod;
            }
        }

        public static Vector3 SideSlider(int id,
                                         Vector3 position,
                                         Vector3 sideVector,
                                         Vector3 direction,
                                         float size,
                                         Handles.CapFunction capFunction,
                                         Vector2 snap,
                                         float bias)
        {
            return (Vector3)SideSliderMethod.Invoke(null, new object[] { id, position, sideVector, direction, size, capFunction, snap, bias });
        }

        private static MethodInfo s_DrawPolyLineWithShadowMethod;

        public static MethodInfo DrawPolyLineWithShadowMethod
        {
            get
            {
                if (s_DrawPolyLineWithShadowMethod == null)
                {
                    s_DrawPolyLineWithShadowMethod = ClassType.GetMethod("DrawPolyLineWithShadow", BindingFlags.Static | BindingFlags.Public);
                }
                if (s_DrawPolyLineWithShadowMethod == null)
                {
                    throw new NullReferenceException("The RectHandles.DrawPolyLineWithShadow is not exist.");
                }
                return s_DrawPolyLineWithShadowMethod;
            }
        }

        public static void DrawPolyLineWithShadow(Color shadowColor,
                                                  Vector2 screenOffset,
                                                  params Vector3[] points)
        {
            DrawPolyLineWithShadowMethod.Invoke(null, new object[] { shadowColor, screenOffset, points });
        }

        private static MethodInfo s_DrawDottedLineWithShadowMethod;

        public static MethodInfo DrawDottedLineWithShadowMethod
        {
            get
            {
                if (s_DrawDottedLineWithShadowMethod == null)
                {
                    s_DrawDottedLineWithShadowMethod = ClassType.GetMethod("DrawDottedLineWithShadow", BindingFlags.Static | BindingFlags.Public, null, new Type[]
                    {
                        typeof(Color),
                        typeof(Vector2),
                        typeof(Vector3),
                        typeof(Vector3),
                        typeof(float)
                    }, null);
                }
                if (s_DrawDottedLineWithShadowMethod == null)
                {
                    throw new NullReferenceException("The RectHandles.DrawDottedLineWithShadow is not exist.");
                }
                return s_DrawDottedLineWithShadowMethod;
            }
        }

        public static void DrawDottedLineWithShadow(Color shadowColor,
                                                    Vector2 screenOffset,
                                                    Vector3 p1,
                                                    Vector3 p2,
                                                    float screenSpaceSize)
        {
            DrawDottedLineWithShadowMethod.Invoke(null, new object[] { shadowColor, screenOffset, p1, p2, screenSpaceSize });
        }
    }
}