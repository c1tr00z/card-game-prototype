using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using System.Linq;

namespace CustomMeshes {
    [ExecuteInEditMode]
    public class GUIMesh3D : Graphic {

        private Vector2 _centerOffset;
        private Vector3 _fillScale = Vector3.one;

        [SerializeField]
        private Mesh _mesh;
        [SerializeField]
        private bool _saveRatio;

        [SerializeField]
        private Vector2 _offset;

        [SerializeField]
        protected bool _randomVertexColor;

        protected override void OnPopulateMesh(VertexHelper vh) {
            if (_mesh == null) {
                base.OnPopulateMesh(vh);
                return;
            }
            vh.Clear();
            
            RecalculateOffset();

            RecalculateScale();

            _mesh.vertices.ToList().ForEach(v => {
                var vert = UIVertex.simpleVert;
                vert.color = !_randomVertexColor ? color : new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                vert.position = new Vector3(v.x * _fillScale.x, v.y * _fillScale.y, v.z * _fillScale.z) + _centerOffset.ToVector3() + _offset.ToVector3();
                vh.AddVert(vert);
            });

            List<int> points = new List<int>();
            _mesh.triangles.ToList().ForEach(tp => {
                if (points.Count == 3) {
                    vh.AddTriangle(points[0], points[1], points[2]);
                    points.Clear();
                }
                points.Add(tp);
            });
            vh.AddTriangle(points[0], points[1], points[2]);
        }

        public void RecalculateOffset() {
            _centerOffset = new Vector2(0 - _mesh.vertices.Select(v => v.x).Average(), 0 - _mesh.vertices.Select(v => v.y).Average());
        }

        public void RecalculateScale() {

            Vector2 corner1 = Vector2.zero;
            Vector2 corner2 = Vector2.zero;

            corner1.x = 0f;
            corner1.y = 0f;
            corner2.x = 1f;
            corner2.y = 1f;

            corner1.x -= rectTransform.pivot.x;
            corner1.y -= rectTransform.pivot.y;
            corner2.x -= rectTransform.pivot.x;
            corner2.y -= rectTransform.pivot.y;

            corner1.x *= rectTransform.rect.width;
            corner1.y *= rectTransform.rect.height;
            corner2.x *= rectTransform.rect.width;
            corner2.y *= rectTransform.rect.height;

            var minX = _mesh.vertices.Select(v => v.x).Min(x => x);
            var minY = _mesh.vertices.Select(v => v.y).Min(y => y);
            
            var maxX = _mesh.vertices.Select(v => v.x).Max(x => x);
            var maxY = _mesh.vertices.Select(v => v.y).Max(y => y);

            _fillScale = new Vector2(Mathf.Abs(maxX) > Mathf.Abs(minX) ? corner2.x / maxX : corner1.x / minX,
                                    Mathf.Abs(maxY) > Mathf.Abs(minY) ? corner2.y / maxY : corner1.y / minY);

            if (_saveRatio) {
                _fillScale = Mathf.Abs(_fillScale.x) > Mathf.Abs(_fillScale.y) ? new Vector3(_fillScale.y, _fillScale.y, _fillScale.y) : new Vector3(_fillScale.x, _fillScale.x, _fillScale.x);
            }

            RecalculateOffset();
        }
    }
}