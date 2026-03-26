using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TransformEnhancer.Editor
{
    [CustomEditor(typeof(RectTransform), true)]
    [CanEditMultipleObjects]
    public class EnhancedRectTransformEditor : ImitatedRectTransformEditor
    {
        protected RectTransform m_RectTransform;
        protected SerializedProperty m_LocalPosition;
        protected SerializedProperty m_AnchorMin;
        protected SerializedProperty m_AnchorMax;
        protected SerializedProperty m_AnchoredPosition;
        protected SerializedProperty m_SizeDelta;
        protected SerializedProperty m_Pivot;
        protected SerializedProperty m_LocalRotation;
        protected SerializedProperty m_LocalEulerAngles;
        protected SerializedProperty m_LocalScale;

        private static class Styles
        {
            public static GUIContent ResetPositionAndSizeContent = EditorGUIUtility.TrTextContent("↺", "Reset anchored position and size.");
            public static GUIContent ResetRotationContent = EditorGUIUtility.TrTextContent("↺", "Reset local rotation.");
            public static GUIContent ResetScaleContent = EditorGUIUtility.TrTextContent("↺", "Reset local scale.");
            public static GUIContent RoundContent = EditorGUIUtility.TrTextContent("≈", "Rounded to the nearest ten percent of an integer.");

            public static float ButtonWidth = 20f;
            public static float ButtonHeight = EditorGUIUtility.singleLineHeight;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            m_RectTransform = (RectTransform)target;
            m_LocalPosition = serializedObject.FindProperty("m_LocalPosition");
            m_AnchorMin = serializedObject.FindProperty("m_AnchorMin");
            m_AnchorMax = serializedObject.FindProperty("m_AnchorMax");
            m_AnchoredPosition = serializedObject.FindProperty("m_AnchoredPosition");
            m_SizeDelta = serializedObject.FindProperty("m_SizeDelta");
            m_Pivot = serializedObject.FindProperty("m_Pivot");
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

        // Position And Size
        protected override void OnPrePositionAndSizeGUI(bool anyWithoutParent, bool anyDrivenX, bool anyDrivenY)
        {
            base.OnPrePositionAndSizeGUI(anyWithoutParent, anyDrivenX, anyDrivenY);

            EditorGUILayout.BeginHorizontal();
        }

        protected override void OnPostPositionAndSizeGUI(bool anyWithoutParent, bool anyDrivenX, bool anyDrivenY)
        {
            base.OnPostPositionAndSizeGUI(anyWithoutParent, anyDrivenX, anyDrivenY);

            DrawButton(out var reset, Styles.ResetPositionAndSizeContent, out var round, Styles.RoundContent, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);

            bool anyStretchX = targets.Any(x => (x as RectTransform).anchorMin.x != (x as RectTransform).anchorMax.x);
            bool anyStretchY = targets.Any(x => (x as RectTransform).anchorMin.y != (x as RectTransform).anchorMax.y);
            bool anyNonStretchX = targets.Any(x => (x as RectTransform).anchorMin.x == (x as RectTransform).anchorMax.x);
            bool anyNonStretchY = targets.Any(x => (x as RectTransform).anchorMin.y == (x as RectTransform).anchorMax.y);

            if (reset)
            {
                ResetAnchoredPositionAndSize(anyWithoutParent, anyDrivenX, anyDrivenY, anyStretchX, anyStretchY, anyNonStretchX, anyNonStretchY);
            }

            if (round)
            {
                RoundAnchoredPositionAndSize(anyWithoutParent, anyDrivenX, anyDrivenY, anyStretchX, anyStretchY, anyNonStretchX, anyNonStretchY);
            }

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void ResetAnchoredPositionAndSize(bool anyWithoutParent, bool anyDrivenX, bool anyDrivenY, bool anyStretchX, bool anyStretchY, bool anyNonStretchX, bool anyNonStretchY)
        {
            if (m_LocalPosition.hasMultipleDifferentValues
                || m_AnchorMin.hasMultipleDifferentValues
                || m_AnchorMax.hasMultipleDifferentValues
                || m_AnchoredPosition.hasMultipleDifferentValues
                || m_SizeDelta.hasMultipleDifferentValues
                || m_Pivot.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var obj = new SerializedObject(targetObject);
                    var property1 = obj.FindProperty(m_LocalPosition.propertyPath);
                    property1.vector3Value = Vector3.zero;
                    var property2 = obj.FindProperty(m_AnchorMin.propertyPath);
                    property2.vector2Value = Vector2.one * 0.5f;
                    var property3 = obj.FindProperty(m_AnchorMax.propertyPath);
                    property3.vector2Value = Vector2.one * 0.5f;
                    var property4 = obj.FindProperty(m_AnchoredPosition.propertyPath);
                    property4.vector2Value = Vector2.zero;
                    var property5 = obj.FindProperty(m_SizeDelta.propertyPath);
                    property5.vector2Value = Vector2.one * 100f;
                    var property6 = obj.FindProperty(m_Pivot.propertyPath);
                    property6.vector2Value = Vector2.one * 0.5f;
                    obj.ApplyModifiedProperties();
                }
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
            }
            else
            {
                m_LocalPosition.vector3Value = Vector3.zero;
                m_AnchorMin.vector2Value = Vector2.one * 0.5f;
                m_AnchorMax.vector2Value = Vector2.one * 0.5f;
                m_AnchoredPosition.vector2Value = Vector2.zero;
                m_SizeDelta.vector2Value = Vector2.one * 100f;
                m_Pivot.vector2Value = Vector2.one * 0.5f;
            }
        }

        protected virtual void RoundAnchoredPositionAndSize(bool anyWithoutParent, bool anyDrivenX, bool anyDrivenY, bool anyStretchX, bool anyStretchY, bool anyNonStretchX, bool anyNonStretchY)
        {
            Undo.RecordObjects(serializedObject.targetObjects, "Round Anchored Position and Size");

            if (m_LocalPosition.hasMultipleDifferentValues
                || m_AnchorMin.hasMultipleDifferentValues
                || m_AnchorMax.hasMultipleDifferentValues
                || m_AnchoredPosition.hasMultipleDifferentValues
                || m_SizeDelta.hasMultipleDifferentValues
                || m_Pivot.hasMultipleDifferentValues)
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    var rectTransform = targetObject as RectTransform;
                    RoundAnchoredPositionAndSizeInternal(rectTransform, anyWithoutParent, anyDrivenX, anyDrivenY, anyStretchX, anyStretchY, anyNonStretchX, anyNonStretchY);
                }
            }
            else
            {
                RoundAnchoredPositionAndSizeInternal(m_RectTransform, anyWithoutParent, anyDrivenX, anyDrivenY, anyStretchX, anyStretchY, anyNonStretchX, anyNonStretchY);
            }
        }

        protected virtual void RoundAnchoredPositionAndSizeInternal(RectTransform rectTransform, bool anyWithoutParent, bool anyDrivenX, bool anyDrivenY, bool anyStretchX, bool anyStretchY, bool anyNonStretchX, bool anyNonStretchY)
        {
            // Pos X + Width
            if (anyNonStretchX || anyWithoutParent || anyDrivenX)
            {
                var anchoredPosition = rectTransform.anchoredPosition;
                rectTransform.anchoredPosition = new Vector2(Mathf.RoundToInt(anchoredPosition.x), anchoredPosition.y);
                var sizeDelta = rectTransform.sizeDelta;
                rectTransform.sizeDelta = new Vector2(Mathf.RoundToInt(sizeDelta.x), sizeDelta.y);
            }
            // Left + Right
            else
            {
                var offsetMin = rectTransform.offsetMin;
                rectTransform.offsetMin = new Vector2(Mathf.RoundToInt(offsetMin.x), offsetMin.y);
                var offsetMax = rectTransform.offsetMax;
                rectTransform.offsetMax = new Vector2(Mathf.RoundToInt(offsetMax.x), offsetMax.y);
            }

            // Pos Y
            if (anyNonStretchY || anyWithoutParent || anyDrivenY)
            {
                var anchoredPosition = rectTransform.anchoredPosition;
                rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, Mathf.RoundToInt(anchoredPosition.y));
                var sizeDelta = rectTransform.sizeDelta;
                rectTransform.sizeDelta = new Vector2(sizeDelta.x, Mathf.RoundToInt(sizeDelta.y));
            }
            // Top + Bottom
            else
            {
                var offsetMin = rectTransform.offsetMin;
                rectTransform.offsetMin = new Vector2(offsetMin.x, Mathf.RoundToInt(offsetMin.y));
                var offsetMax = rectTransform.offsetMax;
                rectTransform.offsetMax = new Vector2(offsetMax.x, Mathf.RoundToInt(offsetMax.y));
            }

            // Pos Z
            var localPosition = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(localPosition.x, localPosition.y, Mathf.RoundToInt(localPosition.z));
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
                var eulerAngles = m_RectTransform.localEulerAngles;
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