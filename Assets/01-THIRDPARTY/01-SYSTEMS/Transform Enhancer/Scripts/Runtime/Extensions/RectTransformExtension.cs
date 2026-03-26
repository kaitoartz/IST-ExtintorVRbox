using UnityEngine;

namespace TransformEnhancer.Runtime
{
    public static class RectTransformExtension
    {
        public static Object GetDrivenByObject(this RectTransform instance)
        {
            return RectTransformReflection.GetDrivenByObject(instance);
        }

        public static DrivenTransformProperties GetDrivenProperties(this RectTransform instance)
        {
            return RectTransformReflection.GetDrivenProperties(instance);
        }

        public static Rect GetRectInParentSpace(this RectTransform instance)
        {
            return RectTransformReflection.GetRectInParentSpace(instance);
        }
    }
}