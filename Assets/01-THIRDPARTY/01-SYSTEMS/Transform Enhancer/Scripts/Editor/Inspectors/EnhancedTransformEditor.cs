using UnityEditor;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    [CustomEditor(typeof(Transform), true)]
    [CanEditMultipleObjects]
    public class EnhancedTransformEditor : ImitatedTransformEditor
    {
        protected Transform m_Transform;
        protected SerializedProperty m_LocalPosition;
        protected SerializedProperty m_LocalRotation;
        protected SerializedProperty m_LocalEulerAngles;
        protected SerializedProperty m_LocalScale;

        private static class Styles
        {
            public static GUIContent ResetPositionContent = EditorGUIUtility.TrTextContent("↺", "Reset local position.");
            public static GUIContent ResetRotationContent = EditorGUIUtility.TrTextContent("↺", "Reset local rotation.");
            public static GUIContent ResetScaleContent = EditorGUIUtility.TrTextContent("↺", "Reset local scale.");
            public static GUIContent RoundContent = EditorGUIUtility.TrTextContent("≈", "Rounded to the nearest ten percent of an integer.");

            public static float ButtonWidth = 20f;
            public static float ButtonHeight = EditorGUIUtility.singleLineHeight;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            m_Transform = (Transform)target;
            m_LocalPosition = serializedObject.FindProperty("m_LocalPosition");
            m_LocalRotation = serializedObject.FindProperty("m_LocalRotation");
            m_LocalEulerAngles = serializedObject.FindProperty("m_LocalEulerAnglesHint");
            m_LocalScale = serializedObject.FindProperty("m_LocalScale");
        }

        protected virtual void DrawButton(out bool reset, GUIContent contentReset, out bool round, GUIContent contentRound, float height, float topPadding = 0f, float interval = 0f)
        {
            var rect = EditorGUILayout.GetControlRect(false, height, GUILayout.Width(Styles.ButtonWidth * 2));
            rect.y += topPadding;
            rect.width = Styles.ButtonWidth;
            rect.height = Styles.ButtonHeight;
            reset = GUI.Button(rect, contentReset);
            rect.x += Styles.ButtonWidth + interval;
            round = GUI.Button(rect, contentRound);
        }

        // Position
        protected override void OnPrePositionGUI()
        {
            base.OnPrePositionGUI();

            EditorGUILayout.BeginHorizontal();
        }

        protected override void OnPostPositionGUI()
        {
            base.OnPostPositionGUI();

            DrawButton(out var reset, Styles.ResetPositionContent, out var round, Styles.RoundContent, EditorGUIUtility.singleLineHeight);

            if (reset)
            {
                ResetLocalPosition();
            }

            if (round)
            {
                RoundLocalPosition();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void ResetLocalPosition()
        {
            if (m_LocalPosition.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var obj = new SerializedObject(targetObject);
                    var property = obj.FindProperty(m_LocalPosition.propertyPath);
                    property.vector3Value = Vector3.zero;
                    obj.ApplyModifiedProperties();
                }
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
            }
            else
            {
                m_LocalPosition.vector3Value = Vector3.zero;
            }
        }

        protected virtual void RoundLocalPosition()
        {
            if (m_LocalPosition.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var obj = new SerializedObject(targetObject);
                    var property = obj.FindProperty(m_LocalPosition.propertyPath);
                    var position = property.vector3Value;
                    var x = Mathf.RoundToInt(position.x);
                    var y = Mathf.RoundToInt(position.y);
                    var z = Mathf.RoundToInt(position.z);
                    property.vector3Value = new Vector3(x, y, z);
                    obj.ApplyModifiedProperties();
                }
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
            }
            else
            {
                var position = m_LocalPosition.vector3Value;
                var x = Mathf.RoundToInt(position.x);
                var y = Mathf.RoundToInt(position.y);
                var z = Mathf.RoundToInt(position.z);
                m_LocalPosition.vector3Value = new Vector3(x, y, z);
            }
        }

        // Rotation
        protected override void OnPreRotationGUI()
        {
            base.OnPreRotationGUI();

            EditorGUILayout.BeginHorizontal();
        }

        protected override void OnPostRotationGUI()
        {
            base.OnPostRotationGUI();

            DrawButton(out var reset, Styles.ResetRotationContent, out var round, Styles.RoundContent, EditorGUIUtility.singleLineHeight);

            if (reset)
            {
                ResetLocalRotation();
            }

            if (round)
            {
                RoundLocalRotation();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void ResetLocalRotation()
        {
            if (m_LocalRotation.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var obj = new SerializedObject(targetObject);
                    var property1 = obj.FindProperty(m_LocalRotation.propertyPath);
                    property1.quaternionValue = Quaternion.identity;
                    var property2 = obj.FindProperty(m_LocalEulerAngles.propertyPath);
                    property2.vector3Value = Vector3.zero;
                    obj.ApplyModifiedProperties();
                }
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
            }
            else
            {
                m_LocalRotation.quaternionValue = new Quaternion(0, 0, 0, 0);
                m_LocalEulerAngles.vector3Value = Vector3.zero;
            }
        }

        protected virtual void RoundLocalRotation()
        {
            if (m_LocalRotation.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var obj = new SerializedObject(targetObject);
                    var property = obj.FindProperty(m_LocalRotation.propertyPath);
                    var eulerAngles = ((Transform)targetObject).localEulerAngles;
                    var x = Mathf.RoundToInt(eulerAngles.x);
                    var y = Mathf.RoundToInt(eulerAngles.y);
                    var z = Mathf.RoundToInt(eulerAngles.z);
                    property.quaternionValue = Quaternion.Euler(x, y, z);
                    obj.ApplyModifiedProperties();
                }

                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
            }
            else
            {
                var eulerAngles = m_Transform.localEulerAngles;
                var x = Mathf.RoundToInt(eulerAngles.x);
                var y = Mathf.RoundToInt(eulerAngles.y);
                var z = Mathf.RoundToInt(eulerAngles.z);
                m_LocalRotation.quaternionValue = Quaternion.Euler(x, y, z);
            }
        }

        // Scale
        protected override void OnPreScaleGUI()
        {
            base.OnPreScaleGUI();

            EditorGUILayout.BeginHorizontal();
        }

        protected override void OnPostScaleGUI()
        {
            base.OnPostScaleGUI();

            DrawButton(out var reset, Styles.ResetScaleContent, out var round, Styles.RoundContent, EditorGUIUtility.singleLineHeight);

            if (reset)
            {
                ResetLocalScale();
            }

            if (round)
            {
                RoundLocalScale();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void ResetLocalScale()
        {
            if (m_LocalScale.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var obj = new SerializedObject(targetObject);
                    var property = obj.FindProperty(m_LocalScale.propertyPath);
                    property.vector3Value = Vector3.one;
                    obj.ApplyModifiedProperties();
                }
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
            }
            else
            {
                m_LocalScale.vector3Value = Vector3.one;
            }
        }

        protected virtual void RoundLocalScale()
        {
            if (m_LocalScale.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var obj = new SerializedObject(targetObject);
                    var property = obj.FindProperty(m_LocalScale.propertyPath);
                    var scale = property.vector3Value;
                    var x = Mathf.RoundToInt(scale.x);
                    var y = Mathf.RoundToInt(scale.y);
                    var z = Mathf.RoundToInt(scale.z);
                    property.vector3Value = new Vector3(x, y, z);
                    obj.ApplyModifiedProperties();
                }
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
            }
            else
            {
                var scale = m_LocalScale.vector3Value;
                var x = Mathf.RoundToInt(scale.x);
                var y = Mathf.RoundToInt(scale.y);
                var z = Mathf.RoundToInt(scale.z);
                m_LocalScale.vector3Value = new Vector3(x, y, z);
            }
        }
    }
}