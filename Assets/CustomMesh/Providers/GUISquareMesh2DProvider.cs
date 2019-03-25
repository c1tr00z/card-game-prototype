using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace CustomMeshes {
    public class GUISquareMesh2DProvider : GUICalculatedMeshProvider {

        class Corner {
            public Vector2 center;
            public List<Vector2> points;
            public Vector2 first { get { return points[0]; } }
            public Vector2 last { get { return points[points.Count - 1]; } }
            public List<int> pointsIndexes;
            public int firstPointIndex { get { return pointsIndexes[0]; } }
            public int lastPointIndex { get { return pointsIndexes[pointsIndexes.Count - 1]; } }
        }

        [SerializeField]
        private float _roundRadius;
        [SerializeField]
        [Range(0, 32)]
        private int _roundVerticles;

        private List<Corner> _corners;

        private int _roundVerticlesTrue;

        public override float leftSlicedBound { get { return _roundRadius; } }
        public override float rightSlicedBound { get { return _roundRadius; } }
        public override float topSlicedBound { get { return _roundRadius; } }
        public override float bottomSlicedBound { get { return _roundRadius; } }

        public override Vector2[] outerCircuit {
            get {
                var outerEdgeVerticles = new List<Vector2>();
                if (_corners == null) {
                    return new Vector2[0];
                }
                _corners.ForEach(c => {
                    for (var i = c.points.Count - 1; i >= 0; i--) {
                        outerEdgeVerticles.Add(c.points[i]);
                    }
                });
                return outerEdgeVerticles.ToArray();
            }
        }

        public override Vector2 GetUV(int index) {
            var innerUV = (originalBounds.y - _roundRadius) / originalBounds.y;

            if (index == 0) {
                return Vector2.zero;
            } else if (index < 5) {
                return new Vector2((index - 1) % 2 == 0 ? 0 : 1, innerUV);
            } else {
                return new Vector2((index - 1) % 2 == 0 ? 0 : 1, 1);
            }
        }

        //public override UIVertex ProcessVertex(UIVertex vertex, int index) {

        //    var innerUV = (originalBounds.y - _roundRadius) / originalBounds.y;

        //    if (index == 0) {
        //        vertex.uv0 = Vector2.zero;
        //    } else if (index < 5) {
        //        vertex.uv0 = new Vector2((index - 1) % 2 == 0 ? 0 : 1, innerUV);
        //    } else {
        //        vertex.uv0 = new Vector2((index - 1) % 2 == 0 ? 0 : 1, 1);
        //    }

        //    return vertex;
        //}

        public override void Calculate(bool callRefresh) {
            base.Calculate(callRefresh);


            _roundVerticlesTrue = _roundVerticles * 4 + 4;

            _roundRadius = _roundRadius < 0 ? 0 : _roundRadius;

            if (_roundRadius == 0) {
                _verticles = new Vector2[] { new Vector2(originalBounds.x / -2, originalBounds.y / 2), new Vector2(originalBounds.x / 2, originalBounds.y / 2),
                                        new Vector2(originalBounds.x / 2, originalBounds.y / -2), new Vector2(originalBounds.x / -2, originalBounds.y / -2), };

                _triangles = TriangulateVerticles(_verticles).ToList();
            } else {
                FillVerticles();
                FillTriangles();

                var outerEdgeVerticles = new List<Vector2>();
                _corners.ForEach(c => {
                    for (var i = c.points.Count - 1; i >= 0; i--) {
                        outerEdgeVerticles.Add(c.points[i]);
                    }
                });
            }

            if (callRefresh) {
                RefreshRenderer();
            }
        }

        private void FillVerticles() {
            _corners = new List<Corner>();
            _corners.Add(MakeCorner(new Vector2((originalBounds.x / -2) + _roundRadius, originalBounds.y / 2 - _roundRadius), 90));
            _corners.Add(MakeCorner(new Vector2(originalBounds.x / 2 - _roundRadius, originalBounds.y / 2 - _roundRadius), 0));
            _corners.Add(MakeCorner(new Vector2(originalBounds.x / 2 - _roundRadius, originalBounds.y / -2 + _roundRadius), 270));
            _corners.Add(MakeCorner(new Vector2(originalBounds.x / -2 + _roundRadius, originalBounds.y / -2 + _roundRadius), 180));

            var verticlesList = new List<Vector2>();
            verticlesList.Add(Vector2.zero);

            verticlesList.AddRange(_corners.Select(c => c.center));
            var pointIndex = verticlesList.Count;
            _corners.ForEach(c => {
                c.pointsIndexes = new List<int>();
                c.points.ForEach(p => {
                    verticlesList.Add(p);
                    c.pointsIndexes.Add(pointIndex);
                    pointIndex++;
                });
            }
            );

            _verticles = verticlesList.ToArray();
        }

        private void FillTriangles() {
            ClearTriangles();
            //triangles.Add(new Triangle { p1 = 0, p2 = 1, p3 = 2 });
            //triangles.Add(new Triangle { p1 = 0, p2 = 2, p3 = 3 });

            for (var i = 0; i < 4; i++) {
                DrawCorner(i);
            }
        }

        private Corner MakeCorner(Vector2 center, float angleOffset) {
            var corner = new Corner { center = center };
            corner.points = new List<Vector2>();
            var angleStep = 360f / _roundVerticlesTrue;
            for (var i = 0; i < _roundVerticles + 2; i++) {
                corner.points.Add(center + new Vector2(_roundRadius * Mathf.Cos((angleStep * i + angleOffset) * Mathf.Deg2Rad), _roundRadius * Mathf.Sin((angleStep * i + angleOffset) * Mathf.Deg2Rad)));
            }
            return corner;
        }

        private void DrawCorner(int index) {
            var nextIndex = index < _corners.Count - 1 ? index + 1 : 0;
            _triangles.Add(new Triangle { p1 = 0, p2 = index + 1, p3 = nextIndex + 1 });
            _triangles.Add(new Triangle { p1 = _corners[index].firstPointIndex, p2 = _corners[nextIndex].lastPointIndex, p3 = nextIndex + 1 });
            _triangles.Add(new Triangle { p1 = _corners[index].firstPointIndex, p2 = nextIndex + 1, p3 = index + 1 });

            for (var i = _corners[index].pointsIndexes.Count - 1; i > 0; i--) {
                _triangles.Add(new Triangle { p1 = index + 1, p2 = _corners[index].pointsIndexes[i], p3 = _corners[index].pointsIndexes[i - 1] });
            }
        }
    }
}