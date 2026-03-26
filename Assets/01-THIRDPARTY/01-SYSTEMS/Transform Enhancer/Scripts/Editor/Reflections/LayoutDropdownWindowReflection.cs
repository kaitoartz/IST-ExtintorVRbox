using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    public class LayoutDropdownWindowReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = UnityEditorAssembly.UnityEditor.GetType("UnityEditor.LayoutDropdownWindow");
                }
                return s_ClassType;
            }
        }

        private object m_Instance;

        public object Instance => m_Instance;

        private PopupWindowContent m_PopupWindow;

        public PopupWindowContent PopupWindow => m_PopupWindow;

        public EditorWindow EditorWindow => PopupWindow.editorWindow;

        private static ConstructorInfo s_Constructor;

        public static ConstructorInfo Constructor
        {
            get
            {
                if (s_Constructor == null)
                {
                    s_Constructor = ClassType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(SerializedObject) }, null);
                }
                if (s_Constructor == null)
                {
                    throw new NullReferenceException("The LayoutDropdownWindow constructor is not exist.");
                }
                return s_Constructor;
            }
        }

        public LayoutDropdownWindowReflection(SerializedObject so)
        {
            m_Instance = Constructor.Invoke(new object[] { so });
            m_PopupWindow = m_Instance as PopupWindowContent;
        }

        private static MethodInfo s_DrawLayoutModeMethod;

        public static MethodInfo DrawLayoutModeMethod
        {
            get
            {
                if (s_DrawLayoutModeMethod == null)
                {
                    s_DrawLayoutModeMethod = ClassType.GetMethod("DrawLayoutMode", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(Rect),
                        typeof(SerializedProperty),
                        typeof(SerializedProperty),
                        typeof(SerializedProperty),
                        typeof(SerializedProperty)
                    }, null);
                }
                if (s_DrawLayoutModeMethod == null)
                {
                    throw new NullReferenceException("The LayoutDropdownWindow.DrawLayoutMode is not exist.");
                }
                return s_DrawLayoutModeMethod;
            }
        }

        public static void DrawLayoutMode(Rect rect,
                                          SerializedProperty anchorMin,
                                          SerializedProperty anchorMax,
                                          SerializedProperty position,
                                          SerializedProperty sizeDelta)
        {
            DrawLayoutModeMethod.Invoke(null, new object[] { rect, anchorMin, anchorMax, position, sizeDelta });
        }

        private static MethodInfo s_DrawLayoutModeHeadersOutsideRectMethod;

        public static MethodInfo DrawLayoutModeHeadersOutsideRectMethod
        {
            get
            {
                if (s_DrawLayoutModeHeadersOutsideRectMethod == null)
                {
                    s_DrawLayoutModeHeadersOutsideRectMethod = ClassType.GetMethod("DrawLayoutModeHeadersOutsideRect", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(Rect),
                        typeof(SerializedProperty),
                        typeof(SerializedProperty),
                        typeof(SerializedProperty),
                        typeof(SerializedProperty)
                    }, null);
                }
                if (s_DrawLayoutModeHeadersOutsideRectMethod == null)
                {
                    throw new NullReferenceException("The LayoutDropdownWindow.DrawLayoutModeHeadersOutsideRect is not exist.");
                }
                return s_DrawLayoutModeHeadersOutsideRectMethod;
            }
        }

        public static void DrawLayoutModeHeadersOutsideRect(Rect rect,
                                                            SerializedProperty anchorMin,
                                                            SerializedProperty anchorMax,
                                                            SerializedProperty position,
                                                            SerializedProperty sizeDelta)
        {
            DrawLayoutModeHeadersOutsideRectMethod.Invoke(null, new object[] { rect, anchorMin, anchorMax, position, sizeDelta });
        }
    }
}