using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EditorCustom.Tools.Scan
{
    /// <summary>
    /// Provides an editor utility to scan and display GameObjects in the scene 
    /// based on the number of triangles and submeshes in their associated Mesh
    /// </summary>
    public class MeshStatsScanner : EditorWindow
    {
        private int _topCount = 10;
        private readonly int _maxCount = 1000;

        /// <summary>
        /// Struct that holds information about a mesh and its GameObject
        /// </summary>
        private struct MeshInfo
        {
            public GameObject go;
            public int triangleCount;
            public int subMeshCount;
        }

        private readonly List<MeshInfo> _meshDataFilter = new(), _meshDataSkinned = new();
        private Vector2 _scrollPosition;
        private bool _isScanned = false;

        /// <summary>
        /// Adds a menu item under "Tools" to open the Mesh Stats Scanner window.
        /// </summary>
        [MenuItem("Tools/Mesh Stats Scanner")]
        public static void ShowWindow()
        {
            MeshStatsScanner window = GetWindow<MeshStatsScanner>("Mesh Stats Scanner");
            window.position = new Rect(650, 250, 450, 550);
        }

        /// <summary>
        /// Renders the UI for the Mesh Stats Scanner editor window.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label("Scan objects with the most triangles", EditorStyles.boldLabel);

            // Clamp the input value between 1 and the maximum allowed count
            _topCount = Mathf.Clamp(_topCount, 1, _maxCount);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Number of objects", GUILayout.Width(160));
            _topCount = EditorGUILayout.IntField(_topCount, GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            // Initiates the mesh scan
            if (GUILayout.Button("Scan the scene") && !_isScanned)
            {
                ScanScene();
            }

            // Displays scan results for filter
            if (_meshDataFilter.Count > 0)
            {
                int displayCount = Mathf.Min(_topCount, _meshDataFilter.Count);

                GUILayout.Space(20);
                GUILayout.Label("Mesh Filter:", EditorStyles.boldLabel);
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));

                for (int i = 0; i < displayCount; i++)
                {
                    MeshInfo info = _meshDataFilter[i];
                    EditorGUILayout.BeginHorizontal();

                    // Object name is a clickable button that selects and pings the object in the hierarchy
                    if (GUILayout.Button(info.go.name, GUILayout.MaxWidth(200)))
                    {
                        Selection.activeGameObject = info.go;
                        EditorGUIUtility.PingObject(info.go);
                    }

                    GUILayout.Label($"{info.triangleCount} Tris | {info.subMeshCount} Submeshes");
                    EditorGUILayout.EndHorizontal();
                }
            }

            // Displays scan results for skinned
            if (_meshDataSkinned.Count > 0)
            {
                int displayCount = Mathf.Min(_topCount, _meshDataSkinned.Count);

                GUILayout.Space(20);
                GUILayout.Label("Skinned Mesh Renderer:", EditorStyles.boldLabel);

                for (int i = 0; i < displayCount; i++)
                {
                    MeshInfo info = _meshDataSkinned[i];
                    EditorGUILayout.BeginHorizontal();

                    // Object name is a clickable button that selects and pings the object in the hierarchy
                    if (GUILayout.Button(info.go.name, GUILayout.MaxWidth(200)))
                    {
                        Selection.activeGameObject = info.go;
                        EditorGUIUtility.PingObject(info.go);
                    }

                    GUILayout.Label($"{info.triangleCount} Tris | {info.subMeshCount} Submeshes");
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// Scans all MeshFilters in the current scene, collecting triangle and submesh counts.
        /// Results are sorted in descending order of triangle count
        /// </summary>
        private void ScanScene()
        {
            _meshDataFilter.Clear();
            _meshDataSkinned.Clear();

            // Retrieves all MeshFilters and SkinnedMeshRenderers in the scene including inactive ones
            MeshFilter[] meshFilters = FindObjectsByType<MeshFilter>(FindObjectsSortMode.None);
            SkinnedMeshRenderer[] skinnedMeshes = FindObjectsByType<SkinnedMeshRenderer>(FindObjectsSortMode.None);

            foreach (MeshFilter mf in meshFilters)
            {
                if (mf.sharedMesh == null)
                    continue;

                Mesh sharedMesh = mf.sharedMesh;

                MeshInfo info = new()
                {
                    go = mf.gameObject,
                    triangleCount = sharedMesh.triangles.Length / 3,
                    subMeshCount = sharedMesh.subMeshCount
                };

                _meshDataFilter.Add(info);
            }

            foreach (SkinnedMeshRenderer smr in skinnedMeshes)
            {
                if (smr.sharedMesh == null) continue;

                Mesh sharedMesh = smr.sharedMesh;
                int tris = sharedMesh.triangles.Length / 3;
                int subMeshes = sharedMesh.subMeshCount;

                MeshInfo info = new()
                {
                    go = smr.gameObject,
                    triangleCount = tris,
                    subMeshCount = subMeshes
                };
                _meshDataSkinned.Add(info);
            }

            // Sort results by descending triangle count
            _meshDataFilter.Sort((a, b) => b.triangleCount.CompareTo(a.triangleCount));
            _meshDataSkinned.Sort((a, b) => b.triangleCount.CompareTo(a.triangleCount));
            _isScanned = true;
        }
    }
}