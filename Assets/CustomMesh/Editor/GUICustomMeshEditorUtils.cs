using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomMeshes {
    public static class GUICustomMeshEditorUtils {
        public static GUICustomMeshInfo SaveToFile() {
            var path = EditorUtility.SaveFilePanel(
                "Save mesh to file", "",
                "New Mesh Info", null);

            var splitted = path.Split('/');

            var name = splitted[splitted.Length - 1];
            var truePath = path.Replace(Application.dataPath, "Assets").Replace("/" + name, "");

            var asset = AssetDBUtils.CreateScriptableObject<GUICustomMeshInfo>(truePath, name);

            return asset;
        }
    }
}
