# Transform 增强器

**Transform 增强器** 是一个用于 Unity 编辑器的插件，它可以帮助你在编辑器中快速地对物体进行 Reset（重置）、Round（取整） 操作。

并且 **Transform 增强器** 将很容易支持对 Transform 进行额外的扩展。

## 有何效果？
### Transform
![guide_1.1.png](Documentation%7E/guide_1.1.png)
### RectTransform
![guide_1.2.png](Documentation%7E/guide_1.2.png)

## 如何扩展？
### Transform
您可以继承 EnhancedTransformEditor 类，我们开放了一些可以重写的方法。
<br>参考示例（CustomTransformEditorSample）：
```
[CustomEditor(typeof(Transform), true)]
[CanEditMultipleObjects]
public class CustomTransformEditor : EnhancedTransformEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("This is the custom transform editor.", "WhiteLargeCenterLabel");
    }
}
```
![guide_2.1.png](Documentation%7E/guide_2.1.png)
### RectTransform
您可以继承 EnhancedRectTransformEditor 类，我们开放了一些可以重写的方法。
<br>参考示例（CustomRectTransformEditorSample）：
```
[CustomEditor(typeof(RectTransform), true)]
[CanEditMultipleObjects]
public class CustomRectTransformEditorSample : EnhancedRectTransformEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("This is the custom transform editor.", "WhiteLargeCenterLabel");
    }
}
```
![guide_2.2.png](Documentation%7E/guide_2.2.png)

## 未来规划
- 将会尝试开发更多的扩展工具。敬请期待~