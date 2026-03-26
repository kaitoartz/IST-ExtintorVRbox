using TransformEnhancer.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform), true)]
[CanEditMultipleObjects]
public class CustomTransformEditorSample : EnhancedTransformEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("This is the custom transform editor.", "WhiteLargeCenterLabel");
    }

    protected override void DrawButton(out bool reset, GUIContent contentReset, out bool round, GUIContent contentRound, float height, float topPadding = 0, float interval = 0)
    {
        reset = false;
        round = false;
    }

    protected override void OnPositionGUI()
    {
        base.OnPositionGUI();

        GUILayout.Label("PPP", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
    }

    protected override void OnRotationGUI()
    {
        base.OnRotationGUI();

        GUILayout.Label("RRR", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
    }

    protected override void OnScaleGUI()
    {
        base.OnScaleGUI();

        GUILayout.Label("SSS", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
    }
}