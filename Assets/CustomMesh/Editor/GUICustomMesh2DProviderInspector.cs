using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CustomMeshes {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GUICustomMesh2DProvider))]
    public class GUICustomMesh2DProviderInspector : UnityEditor.Editor {

        private GUICustomMesh2DProvider _meshProvider;

        private bool _showVerticles = false;

        private GUICustomMeshInfo _customMeshInfo;

        public override void OnInspectorGUI() {

            _meshProvider = (GUICustomMesh2DProvider)target;

            if (_meshProvider == null) {
                return;
            }

            var boundsProperty = serializedObject.FindProperty("_originBounds");
            EditorGUILayout.PropertyField(boundsProperty, true);
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_leftSlicedBound"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rightSlicedBound"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_topSlicedBound"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_bottomSlicedBound"), true);

            _showVerticles = EditorGUILayout.Foldout(_showVerticles, "Show verticles");

            if (_showVerticles) {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                var verticlesProperty = serializedObject.FindProperty("_verticles");
                EditorGUILayout.PropertyField(verticlesProperty, true);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                var trianglesProperty = serializedObject.FindProperty("_triangles");
                EditorGUILayout.PropertyField(trianglesProperty, true);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            if (_meshProvider.startEditing) {
                if (GUILayout.Button("Stop geometry edit")) {
                    _meshProvider.EditGeometryFinish();
                }
            } else {
                if (GUILayout.Button("Start geometry edit")) {
                    _meshProvider.EditGeometryStart();
                }
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Limit verticles")) {
                _meshProvider.LimitVerticles();
            }
            if (GUILayout.Button("Init verticles")) {
                _meshProvider.InitVerticles();
            }
            if (GUILayout.Button("Triangulate")) {
                _meshProvider.TriangulateSelf();
            }
            EditorGUILayout.EndHorizontal();

            var editStarted = serializedObject.FindProperty("_originBounds");

            var uvOffsetProperty = serializedObject.FindProperty("_uvOffset");
            EditorGUILayout.PropertyField(uvOffsetProperty, true);

            LoadFromAsset();

            if (GUI.changed) {
                serializedObject.ApplyModifiedProperties();
            }

            SaveToFileButton(_meshProvider);
        }

        private void LoadFromAsset() {

            _customMeshInfo = (GUICustomMeshInfo)EditorGUILayout.ObjectField("", _customMeshInfo, typeof(GUICustomMeshInfo), false);

            if (_customMeshInfo != null) {
                if (GUILayout.Button("Load data from asset")) {
                    _meshProvider.LoadFrom(_customMeshInfo);
                }
            }
        }

        public static void SaveToFileButton(GUIMeshProvider provider) {
            if (provider == null) {
                return;
            }
            if (GUILayout.Button("Save to file")) {
                SaveToFile(provider);
            }
        }

        public static void SaveToFile(GUIMeshProvider provider) {
            var path = EditorUtility.SaveFilePanel(
                "Save mesh to file",
                "",
                "New Mesh Info",
                null);

            var splitted = path.Split('/');

            var name = splitted[splitted.Length - 1];
            var truePath = path.Replace(Application.dataPath, "Assets").Replace("/" + name, "");

            var asset = AssetDBUtils.CreateScriptableObject<GUICustomMeshInfo>(truePath, name);
            if (asset != null) {
                asset.verticles = provider.verticles;
                asset.triangles = provider.triangles;
                asset.originalBounds = provider.originalBounds;
                asset.leftSlicedBound = provider.leftSlicedBound;
                asset.rightSlicedBound = provider.rightSlicedBound;
                asset.topSlicedBound = provider.topSlicedBound;
                asset.bottomSlicedBound = provider.bottomSlicedBound;
                EditorUtility.SetDirty(asset);
            }
        }
    }
}