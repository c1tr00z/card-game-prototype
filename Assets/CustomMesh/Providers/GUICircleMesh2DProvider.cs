using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

namespace CustomMeshes {
    [ExecuteInEditMode]
    public class GUICircleMesh2DProvider : GUICalculatedMeshProvider {

        [SerializeField]
        [Range(4, 128)]
        private int _vertexCount;

        [SerializeField]
        private float _anguilarOffset = 90;

        //[SerializeField]
        //private float _innerHoleRadius;

        [SerializeField]
        private bool _saveUV = true;

        private float _radius;
        private float _angleStep;

        //Radial Fill
        [SerializeField]
        private bool _radialFill;
        [SerializeField]
        [Range(0f, 1f)]
        private float _radialFillProgess = 1;
        [SerializeField]
        private bool _forwardRadialFill = true;
        private float _radialFillAngle;

        public override Vector2[] outerCircuit {
            get {
                return _verticles.SubArray(1).ToArray();
            }
        }

        public override Vector2 GetUV(int index) {
            if (index == 0) {
                return new Vector2(0.5f, 0);
            } else {
                return new Vector2(index % 2 == 0 ? 1 : 0, 1);
            }
        }

        private Vector2 GetPoint(float radius, float angle) {
            return new Vector2(radius * Mathf.Cos(Mathf.Deg2Rad * (angle + _anguilarOffset)), radius * Mathf.Sin(Mathf.Deg2Rad * (angle + _anguilarOffset)));
        }

        private float GetPointAngle(Vector2 point, float radius) {
            var center = GetPoint(radius, 0);
            var angle = Mathf.Atan2(center.y - point.y, point.x - center.x) * Mathf.Rad2Deg - _anguilarOffset;
            return angle < 0 ? angle + 360 : angle;
        }

        public override void Calculate(bool callRefresh) {
            base.Calculate(callRefresh);
            if (_vertexCount < 4) {
                _vertexCount = 4;
                return;
            } else if (!_vertexCount.Even()) {
                _vertexCount += 1;
            }

            _radius = originalBounds.x < originalBounds.y ? originalBounds.x / 2 : originalBounds.y / 2;
            _angleStep = 360f / _vertexCount;

            if (_radialFill) {
                _radialFillAngle = _radialFillProgess * 360f;
            }

            if (!_radialFill) {
                FillVerticles();

                FillTriangles();

                if (callRefresh) {
                    RefreshRenderer();
                }
            } else {
                FillVerticles();

                var list = new List<Vector2>();
                if (_forwardRadialFill) {
                    list.AddRange(verticles.Select(v => new Vector2(v.x * -1, v.y)));
                } else {
                    list.AddRange(verticles);
                }
                var p0 = GetPoint(_radius, _radialFillAngle);
                //var p1 = GetPoint(_innerHoleRadius, _radialFillAngle);
                if (_forwardRadialFill) {
                    p0.x *= -1;
                    //p1.x *= -1;
                }

                //if (_innerHoleRadius >= 1) {
                //    list.Add(p1);
                //}
                list.Add(p0);
                _verticles = list.ToArray();

                FillTrianglesRadial();


                if (callRefresh) {
                    RefreshRenderer();
                }
            }
        }

        private void FillVerticles() {
            //if (_innerHoleRadius < 1) {
            //_innerHoleRadius = _innerHoleRadius < 0 ? 0 : _innerHoleRadius;
            _verticles = new Vector2[_vertexCount + 1];
            verticles[0] = new Vector2(0, 0);
            //} else {
            //    _innerHoleRadius = _innerHoleRadius > _radius ? _radius - 1 : _innerHoleRadius;
            //    _verticles = new Vector2[_vertexCount * 2];
            //}

            var realVertexCount = _vertexCount + (_radialFill ? 1 : 0);
            for (var i = 0; i < _vertexCount; i++) {
                //if (_innerHoleRadius < 1) {
                verticles[i + 1] = GetPoint(_radius, _angleStep * i);
                //} else {
                //    verticles[i] = GetPoint(_radius, _angleStep * i);
                //    verticles[i + _vertexCount] = GetPoint(_innerHoleRadius, _angleStep * i);
                //}
            }
        }

        private Vector2 FirstExistedVertex() {
            return verticles.Where(v => GetPointAngle(v, _radius) < _radialFillAngle).FirstOrDefault();
        }

        private void FillTriangles() {
            ClearTriangles();
            //if (_innerHoleRadius < 1) {
            for (var i = 1; i < _vertexCount + 1; i++) {
                if (i < _vertexCount) {
                    _triangles.Add(new Triangle { p1 = 0, p2 = i, p3 = i + 1 });
                } else if (i == _vertexCount) {
                    _triangles.Add(new Triangle { p1 = 0, p2 = i, p3 = 1 });
                }
            }
            //} else {
            //    for (var i = 0; i < _vertexCount; i++) {
            //        if (i == 0) {
            //            triangles.Add(new Triangle { p1 = 0, p2 = _vertexCount - 1, p3 = _vertexCount });
            //            triangles.Add(new Triangle { p1 = _vertexCount, p2 = _vertexCount - 1, p3 = _vertexCount * 2 - 1 });
            //        } else {
            //            triangles.Add(new Triangle { p1 = i - 1, p2 = i, p3 = i + _vertexCount });
            //            triangles.Add(new Triangle { p1 = i - 1, p2 = i + _vertexCount, p3 = i - 1 + _vertexCount });
            //        }
            //    }
            //}
        }

        private void FillTrianglesRadial() {
            ClearTriangles();
            //if (_innerHoleRadius < 1) {
            for (var i = 1; i < _vertexCount + 1; i++) {
                var currentAngle = (i - 1) * _angleStep;
                if (currentAngle < _radialFillAngle) {
                    if (i == _vertexCount) {
                        _triangles.Add(new Triangle { p1 = 0, p2 = i, p3 = i + 1 });
                        _triangles.Add(new Triangle { p1 = 0, p2 = i, p3 = i - 1 });
                    } else {
                        _triangles.Add(new Triangle { p1 = 0, p2 = i, p3 = i - 1 });
                    }
                } else if (currentAngle >= _radialFillAngle && (i - 2) * _angleStep < _radialFillAngle) {
                    _triangles.Add(new Triangle { p1 = 0, p2 = i - 1, p3 = _vertexCount + 1 });
                }
            }
            //} else {
            //    for (var i = 0; i < _vertexCount; i++) {
            //        var currentAngle = i * _angleStep;
            //        if (currentAngle < _radialFillAngle) {
            //            if (i == _vertexCount - 1) {
            //                triangles.Add(new Triangle { p1 = i, p2 = i + _vertexCount, p3 = _vertexCount * 2 });
            //                triangles.Add(new Triangle { p1 = _vertexCount * 2 + 1, p2 = i, p3 = _vertexCount * 2 });
            //            } else if (_angleStep * (i + 1) < _radialFillAngle) {
            //                triangles.Add(new Triangle { p1 = i, p2 = i + _vertexCount + 1, p3 = i + _vertexCount });
            //                triangles.Add(new Triangle { p1 = i, p2 = i + 1, p3 = i + _vertexCount + 1 });
            //            }
            //        } else if (currentAngle >= _radialFillAngle && (i - 1) * _angleStep < _radialFillAngle) {
            //            triangles.Add(new Triangle { p1 = i - 1, p2 = _vertexCount * 2 + 1, p3 = _vertexCount * 2 });
            //            triangles.Add(new Triangle { p1 = i - 1, p2 = _vertexCount * 2, p3 = i + _vertexCount - 1 });
            //        }
            //    }
            //}
        }
    }
}
