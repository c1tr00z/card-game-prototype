using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

namespace CustomMeshes {
    public class GUIMeshRenderer : MaskableGraphic {

        [SerializeField]
        private bool _randomVertexColor;

        [SerializeField]
        private int _innerEdgeWidth;

        [SerializeField]
        private bool _sliced;

        [SerializeField]
        private Sprite _sprite;
        
        private Vector2[] _verticlesToRender;
        private List<Triangle> _trianglesToRender;

        private List<Vector2> _verticlesToSave;
        private List<Triangle> _trianglesToSave;

        public bool usePivot = false;

        public IGUIMeshProvider provider { get { return gameObject.GetComponent<IGUIMeshProvider>(); } }

        public bool isProviderEnabled { get { return provider != null && provider.isProviderEnabled; } }

        public override Texture mainTexture {
            get {
                if (_sprite == null) {
                    if (material != null && material.mainTexture != null) {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return _sprite.texture;
            }
        }

        public void RefreshRenderer() {
            _verticlesToRender = new Vector2[0];
            if (_trianglesToRender == null) {
                _trianglesToRender = new List<Triangle>();
            } else {
                _trianglesToRender.Clear();
            }
            if (_verticlesToSave == null) {
                _verticlesToSave = new List<Vector2>();
            } else {
                _verticlesToSave.Clear();
            }
            if (_trianglesToSave == null) {
                _trianglesToSave = new List<Triangle>();
            } else {
                _trianglesToSave.Clear();
            }
            UpdateGeometry();
        }

        protected override void OnPopulateMesh(VertexHelper vh) {

            vh.Clear();
            
            if (provider == null || !provider.isProviderEnabled) {
                return;
            }

            provider.Calculate(false);

            if (provider.verticles == null || provider.verticles.Length < 3) {
                return;
            }

            var xScale = rectTransform.rect.width / provider.originalBounds.x;
            var yScale = rectTransform.rect.height / provider.originalBounds.y;

            if (_innerEdgeWidth >= 1) {
                CutCenter(provider.outerCircuit);
            } else {
                _innerEdgeWidth = 0;
                _verticlesToRender = provider.verticles;
                _trianglesToRender = provider.triangles.ToList();
            }

            if (_verticlesToSave == null) {
                _verticlesToSave = new List<Vector2>();
            } else {
                _verticlesToSave.Clear();
            }

            if (_trianglesToSave == null) {
                _trianglesToSave = new List<Triangle>();
            } else {
                _trianglesToSave.Clear();
            }

            for (var i = 0; i < _verticlesToRender.Length; i++) {
                var v = _verticlesToRender[i];
                var vert = UIVertex.simpleVert;
                vert.color = !_randomVertexColor ? color : new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                var scaledPosition = new Vector2(v.x * xScale, v.y * yScale);

                if (_sliced) {
                    var minSize = new Vector2(provider.leftSlicedBound + provider.rightSlicedBound, provider.topSlicedBound + provider.bottomSlicedBound);

                    var xEr = provider.originalBounds.x / 2 * 0.01f;
                    var yEr = provider.originalBounds.y / 2 * 0.01f;

                    if (v.x - xEr <= (-1 * provider.originalBounds.x / 2) + provider.leftSlicedBound) {
                        scaledPosition.x = -1 * rectTransform.rect.width / 2 - (-1 * provider.originalBounds.x / 2 - v.x);
                        scaledPosition.x = xScale * provider.originalBounds.x < (provider.leftSlicedBound + provider.rightSlicedBound) ? (v.x + ((provider.originalBounds.x / 2) - provider.leftSlicedBound)) : scaledPosition.x;
                    } else if (v.x + xEr >= (provider.originalBounds.x / 2) - provider.rightSlicedBound) {
                        scaledPosition.x = rectTransform.rect.width / 2 - (provider.originalBounds.x / 2 - v.x);
                        scaledPosition.x = xScale * provider.originalBounds.x < (provider.leftSlicedBound + provider.rightSlicedBound) ? (v.x - ((provider.originalBounds.x / 2) - provider.rightSlicedBound)) : scaledPosition.x;
                    }

                    if (v.y - yEr <= (-1 * provider.originalBounds.y / 2) + provider.bottomSlicedBound) {
                        scaledPosition.y = -1 * rectTransform.rect.height / 2 - (-1 * provider.originalBounds.y / 2 - v.y);
                        scaledPosition.y = yScale * provider.originalBounds.y < (provider.topSlicedBound + provider.bottomSlicedBound) ? (v.y + ((provider.originalBounds.y / 2) - provider.bottomSlicedBound)) : scaledPosition.y;
                    } else if (v.y + yEr >= (provider.originalBounds.y / 2) - provider.topSlicedBound) {
                        scaledPosition.y = rectTransform.rect.height / 2 - (provider.originalBounds.y / 2 - v.y);
                        scaledPosition.y = yScale * provider.originalBounds.y < (provider.topSlicedBound + provider.bottomSlicedBound) ? (v.y - ((provider.originalBounds.y / 2) - provider.topSlicedBound)) : scaledPosition.y;
                    }
                }

                _verticlesToSave.Add(new Vector2 (scaledPosition.x / xScale, scaledPosition.y / yScale));

                scaledPosition.x = scaledPosition.x + provider.originalBounds.x * xScale / 2 - rectTransform.pivot.x * provider.originalBounds.x * xScale;
                scaledPosition.y = scaledPosition.y + provider.originalBounds.y * yScale / 2 - rectTransform.pivot.y * provider.originalBounds.y * yScale;

                vert.position = scaledPosition;
                vert = ProcessVertexBase(vert, i);
                vh.AddVert(vert);
            }

            _trianglesToRender.Where(t => IsValidTriangle(t)).ToList().ForEach(t => {
                vh.AddTriangle(t.p1, t.p2, t.p3);
            });
            _trianglesToSave = _trianglesToRender.ToArray().ToList();
        }

        private UIVertex ProcessVertexBase(UIVertex vertex, int index) {

            Vector2 uv = Vector2.zero;

            if (_innerEdgeWidth < 1) {
                vertex.uv0 = provider.GetUV(index);
            } else {
                System.Func<int, bool> isFirstIndex = i => index < _verticlesToRender.Length / 2;

                vertex.uv0 = new Vector2(index % 2 == 0 ? 0 : 1, isFirstIndex(index) ? 1 : 0);
            }

            return vertex;
        }

        private bool IsValidTriangle(Triangle t) {
            System.Func<int, bool> validVerticle = (v) => { return v > -1 && v < _verticlesToRender.Length; };
            return validVerticle(t.p1) && validVerticle(t.p2) && validVerticle(t.p3);
        }

        protected void CutCenter(Vector2[] outerEdge) {
            if (_innerEdgeWidth < 1) {
                if (_innerEdgeWidth < 0) {
                    _innerEdgeWidth = 0;
                }
                return;
            }
            var minSide = provider.originalBounds.x < provider.originalBounds.y ? provider.originalBounds.x : provider.originalBounds.y;
            if (_innerEdgeWidth > minSide) {
                _innerEdgeWidth = Mathf.FloorToInt(minSide * 0.99f);
            }
            var multiplier = _innerEdgeWidth / minSide;
            var verticlesList = new List<Vector2>();
            verticlesList.AddRange(outerEdge);
            outerEdge.ToList().ForEach(v => {
                verticlesList.Add(v * multiplier);
            });
            _verticlesToRender = verticlesList.ToArray();

            ClearTriangles();
            for (var i = 0; i < outerEdge.Length; i++) {
                var nextIndex = (i + 1) < outerEdge.Length ? i + 1 : 0;
                _trianglesToRender.Add(new Triangle { p1 = i, p2 = nextIndex, p3 = nextIndex + outerEdge.Length });
                _trianglesToRender.Add(new Triangle { p1 = i, p2 = nextIndex + outerEdge.Length, p3 = i + outerEdge.Length });
            }
        }

        protected void TriangulateVerticles() {
            ClearTriangles();
            var pointsTriangulated = new Triangulator(_verticlesToRender.ToArray()).Triangulate();
            int[] triangle = new int[3];
            for (var i = 0; i < pointsTriangulated.Length; i++) {
                if (i > 0 && i % 3 == 0) {
                    _trianglesToRender.Add(new Triangle { p1 = triangle[0], p2 = triangle[1], p3 = triangle[2] });
                }
                triangle[i % 3] = pointsTriangulated[i];
            }
            _trianglesToRender.Add(new Triangle { p1 = triangle[0], p2 = triangle[1], p3 = triangle[2] });
        }

        protected void ClearTriangles() {
            if (_trianglesToRender == null) {
                _trianglesToRender = new List<Triangle>();
            } else {
                _trianglesToRender.Clear();
            }
        }

#if UNITY_EDITOR
        public void StoreOriginData(GUICustomMeshInfo asset, bool saveUV) {
            asset.originalBounds = provider.originalBounds;
            asset.verticles = _verticlesToSave.ToArray();
            asset.triangles = _trianglesToSave.ToArray();
            asset.leftSlicedBound = provider.leftSlicedBound;
            asset.rightSlicedBound = provider.rightSlicedBound;
            asset.topSlicedBound = provider.topSlicedBound;
            asset.bottomSlicedBound = provider.bottomSlicedBound;
            if (saveUV) {
                var uv = new Vector2[provider.verticles.Length];
                for (var vertexIndex = 0; vertexIndex < provider.verticles.Length; vertexIndex++) {
                    uv[vertexIndex] = provider.GetUV(vertexIndex);
                }
                asset.customUV = uv;
            }
        }

        private void OnDrawGizmos() {
            var selectedGameObject = UnityEditor.Selection.activeObject as GameObject;
            if (UnityEditor.Selection.activeObject == gameObject || (selectedGameObject != null && GetChildren(selectedGameObject.transform).Contains(transform))) {
                if (_sliced && provider != null && provider.isProviderEnabled) {
                    Gizmos.color = Color.magenta;
                    var scale = GetComponentInParent<Canvas>().transform.localScale;
                    var xCoord = rectTransform.position.x - (rectTransform.rect.width / 2 * scale.x);
                    var yCoord = rectTransform.position.y + (rectTransform.rect.height / 2 * scale.y);
                    var trueRect = new Rect(xCoord,
                                            yCoord,
                                            rectTransform.rect.width * scale.x,
                                            rectTransform.rect.height * scale.y);
                    trueRect = new Rect(trueRect.x + trueRect.width / 2 - trueRect.width * rectTransform.pivot.x,
                        trueRect.y + trueRect.height / 2 - trueRect.height * rectTransform.pivot.y,
                        trueRect.width, trueRect.height);
                    Gizmos.DrawLine(new Vector3(trueRect.x + provider.leftSlicedBound * scale.x, trueRect.y, 0), new Vector3(trueRect.x + provider.leftSlicedBound * scale.x, trueRect.y - trueRect.height, 0));
                    Gizmos.DrawLine(new Vector3(trueRect.x + trueRect.width - provider.rightSlicedBound * scale.x, trueRect.y, 0), new Vector3(trueRect.x + trueRect.width - provider.rightSlicedBound * scale.x, trueRect.y - trueRect.height, 0));
                    Gizmos.DrawLine(new Vector3(trueRect.x, trueRect.y - (provider.topSlicedBound * scale.y), 0), new Vector3(trueRect.x + trueRect.width, trueRect.y - (provider.topSlicedBound * scale.y), 0));
                    Gizmos.DrawLine(new Vector3(trueRect.x, trueRect.y - trueRect.height + (provider.bottomSlicedBound * scale.y), 0), new Vector3(trueRect.x + trueRect.width, trueRect.y - trueRect.height + (provider.bottomSlicedBound * scale.y), 0));
                }
            }
        }

        private Transform[] GetChildren(Transform transform) {
            List<Transform> children = new List<Transform>();
            children.AddRange(transform.GetChildren());
            transform.GetChildren().ToList().ForEach(t => {
                children.AddRange(GetChildren(t));
            });
            return children.ToArray();
        }
#endif
    }
}
