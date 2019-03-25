using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMeshes {
    [ExecuteInEditMode]
    public class GUICalculatedMeshProvider : GUIMeshProvider {

        protected Vector2[] _verticles;

        protected List<Triangle> _triangles;

        public override Vector2[] verticles { get { return _verticles; } }
        public override Triangle[] triangles {
            get {
                _triangles = _triangles == null ? new List<Triangle>() : _triangles;
                return _triangles.ToArray();
            }
        }

        protected override void Start() {
            base.Start();
            if (_triangles == null) {
                _triangles = new List<Triangle>();
            }
        }

        protected void ClearTriangles() {
            if (triangles == null) {
                _triangles = new List<Triangle>();
            } else {
                _triangles.Clear();
            }
        }

#if UNITY_EDITOR
        private void Update() {
            if (!Application.isPlaying) {
                Calculate(true);
            }
        }
#endif
    }
}