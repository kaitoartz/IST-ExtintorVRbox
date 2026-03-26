using UnityEditor;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    // [CustomEditor(typeof(UnityEngine.Transform), true)]
    // [CanEditMultipleObjects]
    public partial class ImitatedTransformEditor : UnityEditor.Editor
    {
        private SerializedProperty m_Position;
        private SerializedProperty m_Scale;
        private TransformRotationGUIReflection m_RotationGUI;

        // #if UNITY_2021_1_OR_NEWER
        private SerializedProperty m_ConstrainProportionsScaleProperty;
        private ConstrainProportionsTransformScaleReflection m_ConstrainProportionsScale;
        private bool m_IsScaleDirty;
        // #endif

        private class Contents
        {
            public GUIContent positionContent = EditorGUIUtility.TrTextContent("Position", "The local position of this GameObject relative to the parent.");
            public GUIContent scaleContent = EditorGUIUtility.TrTextContent("Scale", "The local scaling of this GameObject relative to the parent.");
            public string floatingPointWarning = LocalizationDatabaseReflection.GetLocalizedString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");
        }

        private static Contents s_Contents;

        public virtual void OnEnable()
        {
            m_Position = serializedObject.FindProperty("m_LocalPosition");
            m_Scale = serializedObject.FindProperty("m_LocalScale");

            if (m_RotationGUI == null)
            {
                m_RotationGUI = new TransformRotationGUIReflection();
            }
            m_RotationGUI.OnEnable(serializedObject.FindProperty("m_LocalRotation"), EditorGUIUtility.TrTextContent("Rotation", "The local rotation of this GameObject relative to the parent."));

            m_ConstrainProportionsScaleProperty = serializedObject.FindProperty("m_ConstrainProportionsScale");
            if (m_ConstrainProportionsScaleProperty != null)
            {
                m_ConstrainProportionsScale = new ConstrainProportionsTransformScaleReflection(m_Scale.vector3Value);
            }
        }

        public override void OnInspectorGUI()
        {
            if (s_Contents == null)
            {
                s_Contents = new Contents();
            }

            if (!EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = true;
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
            }

            serializedObject.Update();

            Inspector3D();

            // If scale is dirty, update serializedObject to avoid overwriting new value with previous one when applying modifications
            if (m_IsScaleDirty)
            {
                serializedObject.Update();
            }

            // Warning if global position is too large for floating point errors.
            // SanitizeBounds function doesn't even support values beyond 100000
            Transform t = target as Transform;
            Vector3 pos = t.position;
            if (Mathf.Abs(pos.x) > 100000 || Mathf.Abs(pos.y) > 100000 || Mathf.Abs(pos.z) > 100000)
            {
                EditorGUILayout.HelpBox(s_Contents.floatingPointWarning, MessageType.Warning);
            }

            bool saveUndoName = serializedObject.hasModifiedProperties;
            serializedObject.ApplyModifiedProperties();

            if (saveUndoName)
            {
                int selection = SelectionExtension.Count;
                if (t.position != pos)
                {
                    Undo.SetCurrentGroupName(string.Format("Set Position to {0} in {1}", t.localPosition, selection == 1 ? target.name : "Selected Objects"));
                }
                else
                {
                    Undo.SetCurrentGroupName(string.Format("Set Scale to {0} in {1}", t.localScale, selection == 1 ? target.name : "Selected Objects"));
                }
            }

            if (m_IsScaleDirty)
            {
                m_IsScaleDirty = false;
                this.OnForceReloadInspector();
            }
            //resetting label width as it is carried over to other windows
            EditorGUIUtility.labelWidth = 0;
        }

        protected virtual void Inspector3D()
        {
            DrawPosition();
            DrawRotation();
            DrawScale();
        }
    }

    public partial class ImitatedTransformEditor
    {
        // Position
        protected virtual void DrawPosition()
        {
            OnPrePositionGUI();
            OnPositionGUI();
            OnPostPositionGUI();
        }

        protected virtual void OnPrePositionGUI()
        {
        }

        protected virtual void OnPositionGUI()
        {
            EditorGUILayout.PropertyField(m_Position, s_Contents.positionContent);
        }

        protected virtual void OnPostPositionGUI()
        {
        }

        // Rotation
        protected virtual void DrawRotation()
        {
            OnPreRotationGUI();
            OnRotationGUI();
            OnPostRotationGUI();
        }

        protected virtual void OnPreRotationGUI()
        {
        }

        protected virtual void OnRotationGUI()
        {
            m_RotationGUI.RotationField();
        }

        protected virtual void OnPostRotationGUI()
        {
        }

        // Scale
        protected virtual void DrawScale()
        {
            OnPreScaleGUI();
            OnScaleGUI();
            OnPostScaleGUI();
        }

        protected virtual void OnPreScaleGUI()
        {
        }

        protected virtual void OnScaleGUI()
        {
            Transform t = target as Transform;
            if (t != null
                && m_ConstrainProportionsScaleProperty != null
                && m_ConstrainProportionsScale != null
                && m_ConstrainProportionsScale.Initialize(serializedObject.targetObjects))
            {
                //AxisModified values [-1;2] : [none, x, y, z]
                int axisModified = -1;
                var mixedFields = ConstrainProportionsTransformScaleReflection.GetMixedValueFields(m_Scale);
                Vector3 currentScale = m_ConstrainProportionsScale.DoGUI(EditorGUILayout.GetControlRect(true),
                                                                         s_Contents.scaleContent,
                                                                         m_Scale.vector3Value,
                                                                         serializedObject.targetObjects,
                                                                         ref axisModified,
                                                                         m_Scale,
                                                                         m_ConstrainProportionsScaleProperty);
                var mixedFieldsAfterGUI = ConstrainProportionsTransformScaleReflection.GetMixedValueFields(m_Scale);

                if (currentScale != m_Scale.vector3Value || mixedFields != mixedFieldsAfterGUI)
                {
                    if (serializedObject.GetTargetObjectsCount() > 1)
                    {
                        if (mixedFields != mixedFieldsAfterGUI)
                        {
                            axisModified = -1;
                            for (int i = 0; i < 3; i++)
                            {
                                if (ConstrainProportionsTransformScaleReflection.IsBit(mixedFields, i) && !ConstrainProportionsTransformScaleReflection.IsBit(mixedFieldsAfterGUI, i))
                                {
                                    axisModified = i;
                                    break;
                                }
                            }
                        }

                        if (axisModified != -1)
                        {
                            m_IsScaleDirty = ConstrainProportionsTransformScaleReflection.HandleMultiSelectionScaleChanges(m_Scale.vector3Value,
                                                                                                                           currentScale,
                                                                                                                           m_ConstrainProportionsScale.ConstrainProportionsScale,
                                                                                                                           serializedObject.targetObjects,
                                                                                                                           ref axisModified);
                        }
                    }

                    if (currentScale != m_Scale.vector3Value)
                        m_Scale.vector3Value = currentScale;
                }
            }
            else
            {
                EditorGUILayout.PropertyField(m_Scale, s_Contents.scaleContent);
            }
        }

        protected virtual void OnPostScaleGUI()
        {
        }
    }
}