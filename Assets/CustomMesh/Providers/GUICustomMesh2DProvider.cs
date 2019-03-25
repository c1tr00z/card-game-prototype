using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using System.Linq;

namespace CustomMeshes {
    [ExecuteInEditMode]
    public class GUICustomMesh2DProvider : GUIMeshProvider {

        [SerializeField]
        protected Vector2[] _verticles;
        [SerializeField]
        protected List<Triangle> _triangles;

        [SerializeField]
        private float _leftSlicedBound;
        [SerializeField]
        private float _rightSlicedBound;
        [SerializeField]
        private float _topSlicedBound;
        [SerializeField]
        private float _bottomSlicedBound;

        [SerializeField]
        private Vector2 _uvOffset;

        public bool startEditing { get; private set; }
        private PolygonCollider2D _collider;

        private bool _prevSliced;

        private Vector2[] _customVerticles;

        public override Vector2[] verticles { get { return _verticles; } }
        public override Triangle[] triangles {
            get {
                _triangles = _triangles == null ? new List<Triangle>() : _triangles;
                return _triangles.ToArray();
            }
        }

        public override float leftSlicedBound { get { return _leftSlicedBound; } }
        public override float rightSlicedBound { get { return _rightSlicedBound; } }
        public override float topSlicedBound { get { return _topSlicedBound; } }
        public override float bottomSlicedBound { get { return _bottomSlicedBound; } }

        [ContextMenu("Edit polygon")]
        public void EditPolygons() {
            if (!startEditing) {
                EditGeometryStart();
            } else {
                EditGeometryFinish();
            }
        }

        protected override void Start() {
            base.Start();
            InitVerticles();
            if (_triangles.Count == 0) {
                TriangulateSelf();
            }
            _customVerticles = verticles;
        }

        public override Vector2 GetUV(int index) {
            var vertex = verticles[index];
            return new Vector2(((vertex.x / (originalBounds.x / 2)) + 1) / 2, ((vertex.y / (originalBounds.y / 2)) + 1) / 2) + _uvOffset;
        }

        private void UpdateGeometryData() {

            _customVerticles = _collider.GetPath(0).Select(v => FromGeometryEditVector(v)).ToArray();

            _verticles = _customVerticles;

            TriangulateSelf();
        }

        public void EditGeometryStart() {
            if (startEditing) {
                return;
            }

            //_prevSliced = sliced;
            //sliced = false;

            _customVerticles = _customVerticles != null ? _customVerticles : new Vector2[0];
            var toPath = _customVerticles.Select(v => ToGeometryEditVector(v)).ToArray();
            _collider = gameObject.GetComponent<PolygonCollider2D>();
            if (_collider == null) {
                _collider = gameObject.AddComponent<PolygonCollider2D>();
            }
            _collider.SetPath(0, toPath);
            startEditing = true;
        }

        public void EditGeometryFinish() {
            if (!startEditing) {
                return;
            }

            //verticles = verticles.Select(v => new Vector3( v.x * meshScale.x, v.y * meshScale.y, v.z)).ToArray();

            RefreshRenderer();
            if (_collider != null) {
                DestroyImmediate(_collider);
            }
            startEditing = false;
            //sliced = _prevSliced;
        }

#if UNITY_EDITOR
        public void OnColliderUpdate() {
            if (_collider == null) {
                return;
            }

            UpdateGeometryData();
            RefreshRenderer();
        }

        private void Update() {
            if (_customVerticles == null || _customVerticles.Length == 0) {
                _customVerticles = verticles;
            }
            if (startEditing) {
                OnColliderUpdate();
            } else {
                RefreshRenderer();
                //CutCenter(_customVerticles);
            }
        }

        public void LoadFrom(GUICustomMeshInfo meshInfo) {
            _verticles = meshInfo.verticles;
            _triangles = meshInfo.triangles.ToList();
            _originBounds = meshInfo.originalBounds;
            _leftSlicedBound = meshInfo.leftSlicedBound;
            _rightSlicedBound = meshInfo.rightSlicedBound;
            _topSlicedBound = meshInfo.topSlicedBound;
            _bottomSlicedBound = meshInfo.bottomSlicedBound;
        }
#endif

        public void InitVerticles() {
            if (_verticles == null || _verticles.Length == 0) {
                _verticles = new Vector2[] { new Vector2(-50, 50), new Vector2(50, 50), new Vector2(50, -50), new Vector2(-50, -50) };
                TriangulateSelf();
            }
        }

        private Vector2 ToGeometryEditVector(Vector2 originPoint) {
            return new Vector2(originPoint.x * xScale + originalBounds.x * xScale / 2 - rectTransform.pivot.x * originalBounds.x * xScale
                , originPoint.y * yScale + originalBounds.y * yScale / 2 - rectTransform.pivot.y * originalBounds.y * yScale);
            //scaledPosition.x = scaledPosition.x + provider.originalBounds.x * xScale / 2 - rectTransform.pivot.x * provider.originalBounds.x * xScale;
            //scaledPosition.y = scaledPosition.y + provider.originalBounds.y * yScale / 2 - rectTransform.pivot.y * provider.originalBounds.y * yScale;
        }

        private Vector2 FromGeometryEditVector(Vector2 geometryPoint) {
            return new Vector2((geometryPoint.x - originalBounds.x * xScale / 2 + rectTransform.pivot.x * originalBounds.x * xScale) / xScale
                , (geometryPoint.y - originalBounds.y * yScale / 2 + rectTransform.pivot.y * originalBounds.y * yScale) / yScale);
            //return new Vector2(point.x * xScale + originalBounds.x * xScale / 2 - rectTransform.pivot.x * originalBounds.x * xScale
            //    , point.y * yScale + originalBounds.y * yScale / 2 - rectTransform.pivot.y * originalBounds.y * yScale);
        }

        public void LimitVerticles() {
            var reworkedVerticles = new List<Vector2>();
            _verticles.ToList().ForEach(v => {
                v.x = v.x > _originBounds.x / 2
                    ? _originBounds.x / 2
                    : v.x < -_originBounds.x / 2
                        ? -_originBounds.x / 2
                        : v.x;
                v.y = v.y > _originBounds.y / 2
                    ? _originBounds.y / 2
                    : v.y < -_originBounds.y / 2
                        ? -_originBounds.y / 2
                        : v.y;
                reworkedVerticles.Add(v);
            });
            _verticles = reworkedVerticles.ToArray();
        }

        public void TriangulateSelf() {
            _triangles = TriangulateVerticles(_verticles).ToList();
        }
    }
}