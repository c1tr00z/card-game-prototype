using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMeshes {
    public class GUICustomMeshInfo : ScriptableObject {

        public Vector2[] verticles;

        public Triangle[] triangles;

        public Vector2 originalBounds = new Vector2(100, 100);
        
        public float leftSlicedBound;
        public float rightSlicedBound;
        public float topSlicedBound;
        public float bottomSlicedBound;

        public Vector2[] customUV;
    }
}