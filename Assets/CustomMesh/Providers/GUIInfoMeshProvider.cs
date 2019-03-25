using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMeshes {
    [RequireComponent(typeof(GUIMeshRenderer))]
    public class GUIInfoMeshProvider : GUIBaseMeshProvider {

        [SerializeField]
        private GUICustomMeshInfo _source;

        [SerializeField]
        private Vector2 _uvOffset;

        [SerializeField]
        private bool _useCustomUV;

        public override Vector2[] verticles { get { return _source.verticles; } }

        public override Triangle[] triangles { get { return _source.triangles; } }

        public override bool isProviderEnabled { get { return _source != null; } }

        public override Vector2 originalBounds { get { return _source.originalBounds; } }

        public override float leftSlicedBound { get { return _source.leftSlicedBound; } }

        public override float rightSlicedBound { get { return _source.rightSlicedBound; } }

        public override float topSlicedBound { get { return _source.topSlicedBound; } }

        public override float bottomSlicedBound { get { return _source.bottomSlicedBound; } }

        public override Vector2[] outerCircuit { get { return verticles; } }

        public override Vector2 GetUV(int index) {
            if (_useCustomUV) {
                return _source.customUV != null && _source.customUV.Length > index ? _source.customUV[index] : Vector2.zero;
            } else {
                var rectTransform = transform as RectTransform;
                if (rectTransform != null) {
                    var xScale = rectTransform.rect.width / originalBounds.x;
                    var yScale = rectTransform.rect.height / originalBounds.y;
                }

                var vertex = verticles[index];

                return new Vector2(((vertex.x / (originalBounds.x / 2)) + 1) / 2, ((vertex.y / (originalBounds.y / 2)) + 1) / 2) + _uvOffset;
            }
        }

        public override void Calculate(bool callRefresh) { }
    }
}