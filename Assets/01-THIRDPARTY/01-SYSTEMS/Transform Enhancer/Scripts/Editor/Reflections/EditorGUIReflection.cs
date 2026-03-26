using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    public class RecycledTextEditorReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = EditorGUIReflection.ClassType.GetNestedType("RecycledTextEditor", BindingFlags.Public | BindingFlags.NonPublic);
                }
                return s_ClassType;
            }
        }
    }

    // #if !UNITY_2022_0_OR_NEWER
    public class PropertyVisibilityReflection
    {
        private static Type s_ClassType;

        public static Type ClassType
        {
            get
            {
                if (s_ClassType == null)
                {
                    s_ClassType = EditorGUIReflection.ClassType.GetNestedType("PropertyVisibility", BindingFlags.Public | BindingFlags.NonPublic);
                }
                return s_ClassType;
            }
        }

        public static object All => Enum.Parse(ClassType, nameof(All));

        public static object OnlyVisible => Enum.Parse(ClassType, nameof(OnlyVisible));
    }
    // #endif

    public class EditorGUIReflection
    {
        private static Type s_ClassType = typeof(EditorGUI);

        public static Type ClassType => s_ClassType;

        private static FieldInfo s_DelayedTextEditorField;

        public static FieldInfo DelayedTextEditorField
        {
            get
            {
                if (s_DelayedTextEditorField == null)
                {
                    s_DelayedTextEditorField = ClassType.GetField("s_DelayedTextEditor", BindingFlags.Static | BindingFlags.NonPublic);
                }
                // if (s_DelayedTextEditorField == null)
                // {
                //     throw new NullReferenceException("The EditorGUI.s_DelayedTextEditor is not exist.");
                // }
                return s_DelayedTextEditorField;
            }
        }

        // #if UNITY_6000_0_OR_NEWER
        private static PropertyInfo s_DelayedTextEditorProperty;

        public static PropertyInfo DelayedTextEditorProperty
        {
            get
            {
                if (s_DelayedTextEditorProperty == null)
                {
                    s_DelayedTextEditorProperty = ClassType.GetProperty("s_DelayedTextEditor", BindingFlags.Static | BindingFlags.NonPublic);
                }
                if (s_DelayedTextEditorProperty == null)
                {
                    throw new NullReferenceException("The EditorGUI.s_DelayedTextEditor is not exist.");
                }
                return s_DelayedTextEditorProperty;
            }
        }
        // #endif

        public static object DelayedTextEditor
        {
            get
            {
                if (DelayedTextEditorField != null)
                {
                    return DelayedTextEditorField.GetValue(null);
                }
                // #if UNITY_6000_0_OR_NEWER
                if (DelayedTextEditorProperty != null)
                {
                    return DelayedTextEditorProperty.GetValue(null);
                }
                // #endif
                return null;
            }
        }

        private static FieldInfo s_FloatFieldFormatStringField;

        public static FieldInfo FloatFieldFormatStringField
        {
            get
            {
                if (s_FloatFieldFormatStringField == null)
                {
                    s_FloatFieldFormatStringField = ClassType.GetField("kFloatFieldFormatString", BindingFlags.Static | BindingFlags.NonPublic);
                }
                if (s_FloatFieldFormatStringField == null)
                {
                    throw new NullReferenceException("The EditorGUI.kFloatFieldFormatString is not exist.");
                }
                return s_FloatFieldFormatStringField;
            }
        }

        public static string FloatFieldFormatString => (string)FloatFieldFormatStringField.GetValue(null);

        private static FieldInfo s_VerticalSpacingMultiField;

        public static FieldInfo VerticalSpacingMultiField
        {
            get
            {
                if (s_VerticalSpacingMultiField == null)
                {
                    s_VerticalSpacingMultiField = ClassType.GetField("kVerticalSpacingMultiField", BindingFlags.Static | BindingFlags.NonPublic);
                }
                if (s_VerticalSpacingMultiField == null)
                {
                    throw new NullReferenceException("The EditorGUI.kVerticalSpacingMultiField is not exist.");
                }
                return s_VerticalSpacingMultiField;
            }
        }

        private static PropertyInfo s_VerticalSpacingMultiFieldValueField;

        public static float VerticalSpacingMulti
        {
            get
            {
                var obj = VerticalSpacingMultiField.GetValue(null);
                if (s_VerticalSpacingMultiFieldValueField == null)
                {
                    s_VerticalSpacingMultiFieldValueField = obj.GetType().GetProperty("value", BindingFlags.Instance | BindingFlags.Public);
                }
                if (s_VerticalSpacingMultiFieldValueField == null)
                {
                    throw new NullReferenceException("The SVC<float>.value is not exist.");
                }
                return (float)s_VerticalSpacingMultiFieldValueField.GetValue(obj);
            }
        }

        private static MethodInfo s_DoFloatFieldMethod;

        public static MethodInfo DoFloatFieldMethod
        {
            get
            {
                if (s_DoFloatFieldMethod == null)
                {
                    s_DoFloatFieldMethod = ClassType.GetMethod("DoFloatField", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        RecycledTextEditorReflection.ClassType,
                        typeof(Rect),
                        typeof(Rect),
                        typeof(int),
                        typeof(float),
                        typeof(string),
                        typeof(GUIStyle),
                        typeof(bool)
                    }, null);
                }
                if (s_DoFloatFieldMethod == null)
                {
                    throw new NullReferenceException("The EditorGUI.DoFloatField is not exist.");
                }
                return s_DoFloatFieldMethod;
            }
        }

        public static float DoFloatField(object editor,
                                         Rect position,
                                         Rect dragHotZone,
                                         int id,
                                         float value,
                                         string formatString,
                                         GUIStyle style,
                                         bool draggable)
        {
            return (float)DoFloatFieldMethod.Invoke(null, new object[] { editor, position, dragHotZone, id, value, formatString, style, draggable });
        }

        private static MethodInfo s_MultiPropertyFieldInternalMethod;

        public static MethodInfo MultiPropertyFieldInternalMethod
        {
            get
            {
                if (s_MultiPropertyFieldInternalMethod == null)
                {
                    s_MultiPropertyFieldInternalMethod = ClassType.GetMethod("MultiPropertyFieldInternal", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(Rect),
                        typeof(GUIContent[]),
                        typeof(SerializedProperty),
                        PropertyVisibilityReflection.ClassType,
                        typeof(bool[]),
                        typeof(float)
                    }, null);
                }
                if (s_MultiPropertyFieldInternalMethod == null)
                {
                    throw new NullReferenceException("The EditorGUI.MultiPropertyFieldInternal is not exist.");
                }
                return s_MultiPropertyFieldInternalMethod;
            }
        }

        public static void MultiPropertyFieldInternal(Rect position,
                                                      GUIContent[] subLabels,
                                                      SerializedProperty valuesIterator,
                                                      object visibility,
                                                      bool[] disabledMask = null,
                                                      float prefixLabelWidth = -1f)
        {
            MultiPropertyFieldInternalMethod.Invoke(null, new object[] { position, subLabels, valuesIterator, visibility, disabledMask, prefixLabelWidth });
        }

        private static MethodInfo s_MultiFieldPrefixLabelMethod;

        public static MethodInfo MultiFieldPrefixLabelMethod
        {
            get
            {
                if (s_MultiFieldPrefixLabelMethod == null)
                {
                    s_MultiFieldPrefixLabelMethod = ClassType.GetMethod("MultiFieldPrefixLabel", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(Rect),
                        typeof(int),
                        typeof(GUIContent),
                        typeof(int)
                    }, null);
                }
                if (s_MultiFieldPrefixLabelMethod == null)
                {
                    throw new NullReferenceException("The EditorGUI.MultiFieldPrefixLabel is not exist.");
                }
                return s_MultiFieldPrefixLabelMethod;
            }
        }

        public static Rect MultiFieldPrefixLabel(Rect totalPosition, int id, GUIContent label, int columns)
        {
            return (Rect)MultiFieldPrefixLabelMethod.Invoke(null, new object[] { totalPosition, id, label, columns });
        }

        private static MethodInfo s_CalcPrefixLabelWidthMethod;

        public static MethodInfo CalcPrefixLabelWidthMethod
        {
            get
            {
                if (s_CalcPrefixLabelWidthMethod == null)
                {
                    s_CalcPrefixLabelWidthMethod = ClassType.GetMethod("CalcPrefixLabelWidth", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(GUIContent), typeof(GUIStyle) }, null);
                }
                if (s_CalcPrefixLabelWidthMethod == null)
                {
                    throw new NullReferenceException("The EditorGUI.CalcPrefixLabelWidth is not exist.");
                }
                return s_CalcPrefixLabelWidthMethod;
            }
        }

        public static float CalcPrefixLabelWidth(GUIContent label, GUIStyle style = null)
        {
            return (float)CalcPrefixLabelWidthMethod.Invoke(null, new object[] { label, style });
        }
    }
}