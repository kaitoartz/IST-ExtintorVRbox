using UnityEditor;

namespace TransformEnhancer.Editor
{
    public static class SelectionExtension
    {
        public static int Count
        {
            get
            {
#if UNITY_2020_1_OR_NEWER
                return Selection.count;
#else
                return Selection.objects.Length;
#endif
            }
        }
    }
}