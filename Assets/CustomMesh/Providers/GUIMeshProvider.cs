using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMeshes {
    public class GUIMeshProvider : GUIBaseMeshProvider {
        
        [SerializeField]
        protected Vector2 _originBounds = new Vector2 { x = 100, y = 100 };

        public override Vector2[] verticles { get { return new Vector2[0]; } }

        public override Triangle[] triangles { get { return new Triangle[0]; } }

        public override bool isProviderEnabled { get { return true; } }

        public override Vector2 originalBounds { get { return _originBounds; } }

        public override float leftSlicedBound { get { return 0; } }

        public override float rightSlicedBound { get { return 0; } }

        public override float topSlicedBound { get { return 0; } }

        public override float bottomSlicedBound { get { return 0; } }

        public override Vector2[] outerCircuit { get { return verticles; } }

        public float xScale { get { return rectTransform.rect.width / originalBounds.x; } }
        public float yScale { get { return rectTransform.rect.height / originalBounds.y; } }

        protected virtual void Start() { }

        public override Vector2 GetUV(int index) { return Vector2.zero; }

        //public override UIVertex ProcessVertex(UIVertex vertex, int index) { return vertex; }

        public Triangle[] TriangulateVerticles(Vector2[] verticles) {
            //ClearTriangles();
            var triangles = new List<Triangle>();
            var pointsTriangulated = new Triangulator(verticles).Triangulate();
            int[] triangle = new int[3];
            for (var i = 0; i < pointsTriangulated.Length; i++) {
                if (i > 0 && i % 3 == 0) {
                    triangles.Add(new Triangle { p1 = triangle[0], p2 = triangle[1], p3 = triangle[2] });
                }
                triangle[i % 3] = pointsTriangulated[i];
            }
            triangles.Add(new Triangle { p1 = triangle[0], p2 = triangle[1], p3 = triangle[2] });
            return triangles.ToArray();
        }

        protected void RefreshRenderer() {
            if (GetComponent<GUIMeshRenderer>() != null) {
                GetComponent<GUIMeshRenderer>().RefreshRenderer();
            }
        }

        public override void Calculate(bool callRefresh) { }
    }
}
