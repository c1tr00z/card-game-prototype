using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CustomMeshes {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GUICustomMeshInfo))]
    public class GUICustomMeshInfoInspector : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            //OnPreviewGUI(new Rect(0, 0, 300, 300), new GUIStyle());
        }

        public override bool HasPreviewGUI() {
            return true;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            base.OnPreviewGUI(r, background);

            var info = target as GUICustomMeshInfo;

            if (info == null) {
                return;
            }

            var realRect = new Rect(r.x + r.width * .1f, r.y + r.height * .1f, r.width * .8f, r.height * .8f);

            GUI.color = Color.white;

            var scale = new Vector2(realRect.width / info.originalBounds.x, realRect.height / info.originalBounds.y);
            var scaleCoefficient = new float[] { scale.x, scale.y }.Min(e => e);

            info.triangles.ToList().ForEach(t => {
                Drawing.DrawLine(realRect.center + info.verticles[t.p1] * scaleCoefficient, realRect.center + info.verticles[t.p2] * scaleCoefficient, Mathf.Max(scaleCoefficient, 2));
                Drawing.DrawLine(realRect.center + info.verticles[t.p2] * scaleCoefficient, realRect.center + info.verticles[t.p3] * scaleCoefficient, Mathf.Max(scaleCoefficient, 2));
                Drawing.DrawLine(realRect.center + info.verticles[t.p3] * scaleCoefficient, realRect.center + info.verticles[t.p1] * scaleCoefficient, Mathf.Max(scaleCoefficient, 2));
            });
        }
    }
}