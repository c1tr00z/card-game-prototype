using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CustomMeshes {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GUIMeshProvider), true)]
    public class GUIMeshProviderInspector : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var provider = target as GUIMeshProvider;
        }
    }
}