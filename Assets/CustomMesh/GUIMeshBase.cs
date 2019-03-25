using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using UnityEngine.UI;

namespace CustomMeshes {
    public class GUIMeshBase : Graphic {

        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        protected Vector2[] verticles;
        [SerializeField]
        protected List<Triangle> triangles;

        [SerializeField]
        protected Vector2 originalBounds = new Vector2(100, 100);

        [SerializeField]
        private bool _randomVertexColor;

        [SerializeField]
        protected bool sliced;

        [SerializeField]
        private float _leftSlicedBound;
        [SerializeField]
        private float _rightSlicedBound;
        [SerializeField]
        private float _topSlicedBound;
        [SerializeField]
        private float _bottomSlicedBound;

        [SerializeField]
        private int _innerEdgeWidth;

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

        protected Vector2 meshScale { get { return new Vector3(rectTransform.rect.width / originalBounds.x, rectTransform.rect.height / originalBounds.y); } }

        protected override void OnPopulateMesh(VertexHelper vh) {
            if (verticles == null || verticles.Length < 3) {
                ClearGeometryData();
            }

            if (verticles.Length < 3) {
                return;
            }

            vh.Clear();

            var xScale = rectTransform.rect.width / originalBounds.x;
            var yScale = rectTransform.rect.height / originalBounds.y;

            for (var i = 0; i < verticles.Length; i++) {
                var v = verticles[i];
                var vert = UIVertex.simpleVert;
                vert.color = !_randomVertexColor ? color : new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                var scaledPosition = new Vector2(v.x * xScale, v.y * yScale);

                if (sliced) {

                    var minSize = new Vector2(_leftSlicedBound + _rightSlicedBound, _topSlicedBound + _bottomSlicedBound);

                    if (v.x < (-1 * originalBounds.x / 2) + _leftSlicedBound) {
                        scaledPosition.x = -1 * rectTransform.rect.width / 2 - (-1 * originalBounds.x / 2 - v.x);
                    } else if (v.x > (originalBounds.x / 2) - _rightSlicedBound) {
                        scaledPosition.x = rectTransform.rect.width / 2 - (originalBounds.x / 2 - v.x);
                        scaledPosition.x = xScale * originalBounds.x < (_leftSlicedBound + _rightSlicedBound) ? _rightSlicedBound - (originalBounds.x * xScale / 2 - v.x) : scaledPosition.x;
                    }

                    if (v.y < (-1 * originalBounds.y / 2) + _bottomSlicedBound) {
                        scaledPosition.y = -1 * rectTransform.rect.height / 2 - (-1 * originalBounds.y / 2 - v.y);
                    } else if (v.y > (originalBounds.y / 2) - _topSlicedBound) {
                        scaledPosition.y = rectTransform.rect.height / 2 - (originalBounds.y / 2 - v.y);
                        scaledPosition.y = yScale * originalBounds.y < (_topSlicedBound + _bottomSlicedBound) ? _topSlicedBound - (originalBounds.y / 2 - v.y) : scaledPosition.y;
                    }
                }

                vert.position = scaledPosition;
                vert = ProcessVertexBase(vert, i);
                vh.AddVert(vert);
            }

            triangles.Where(t => ValidTriangle(t)).ToList().ForEach(t => {
                vh.AddTriangle(t.p1, t.p2, t.p3);
            });
        }

        private bool ValidTriangle(Triangle t) {
            System.Func<int, bool> validVerticle = (v) => { return v > -1 && v < verticles.Length; };
            return validVerticle(t.p1) && validVerticle(t.p2) && validVerticle(t.p3);
        }

        private UIVertex ProcessVertexBase(UIVertex vertex, int index) {

            if (_innerEdgeWidth < 1) {
                return ProcessVertex(vertex, index);
            }

            System.Func<int, bool> isFirstIndex = i => index < verticles.Length / 2;

            vertex.uv0 = new Vector2(index % 2 == 0 ? 0 : 1, isFirstIndex(index) ? 1 : 0);

            return vertex;
        }

        protected virtual UIVertex ProcessVertex(UIVertex vertex, int index) { return vertex; }

        protected void ClearGeometryData() {
            verticles = new Vector2[] { new Vector2(-50, 50), new Vector2(50, 50), new Vector2(50, -50), new Vector2(-50, -50) };
            //ClearTriangles();
            triangles.Add(new Triangle { p1 = 0, p2 = 1, p3 = 2 });
            triangles.Add(new Triangle { p1 = 2, p2 = 3, p3 = 0 });
        }

        private void OnDrawGizmos() {
            if (sliced) {
                Gizmos.color = Color.red;
                var scale = GetComponentInParent<Canvas>().transform.localScale;
                var xCoord = rectTransform.position.x - (rectTransform.rect.width / 2 * scale.x);
                var yCoord = rectTransform.position.y + (rectTransform.rect.height / 2 * scale.y);
                var trueRect = new Rect(xCoord,
                                        yCoord,
                                        rectTransform.rect.width * scale.x,
                                        rectTransform.rect.height * scale.y);
                Gizmos.DrawLine(new Vector3(trueRect.x + _leftSlicedBound * scale.x, trueRect.y, 0), new Vector3(trueRect.x + _leftSlicedBound * scale.x, trueRect.y - trueRect.y, 0));
                Gizmos.DrawLine(new Vector3(trueRect.x + trueRect.width - _rightSlicedBound * scale.x, trueRect.y, 0), new Vector3(trueRect.x + trueRect.width - _rightSlicedBound * scale.x, trueRect.y - trueRect.height, 0));
                Gizmos.DrawLine(new Vector3(trueRect.x, trueRect.y - (_topSlicedBound * scale.y), 0), new Vector3(trueRect.x + trueRect.width, trueRect.y - (_topSlicedBound * scale.y), 0));
                Gizmos.DrawLine(new Vector3(trueRect.x, trueRect.y - trueRect.height + (_bottomSlicedBound * scale.y), 0), new Vector3(trueRect.x + trueRect.width, trueRect.y - trueRect.height + (_bottomSlicedBound * scale.y), 0));
            }
        }
    }
}
