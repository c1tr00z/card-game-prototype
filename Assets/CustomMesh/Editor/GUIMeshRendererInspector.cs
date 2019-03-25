using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomMeshes {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GUIMeshRenderer), true)]
    public class GUIMeshRendererInspector : UnityEditor.Editor {

        private GUIMeshRenderer _renderer;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            _renderer = target as GUIMeshRenderer;

            if (_renderer == null) {
                return;
            }

            if (GUILayout.Button("Refresh")) {
                _renderer.RefreshRenderer();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save to asset")) {
                SaveToAsset(false);
            }

            if (GUILayout.Button("Save to asset w\\ UV")) {
                SaveToAsset(true);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SaveToAsset(bool saveUV) {
            var asset = GUICustomMeshEditorUtils.SaveToFile();
            _renderer.StoreOriginData(asset, saveUV);
            EditorUtility.SetDirty(asset);
        }
    }
}
