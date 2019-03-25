using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMeshes {
    [RequireComponent(typeof(GUIMeshRenderer))]
    [ExecuteInEditMode]
    public abstract class GUIBaseMeshProvider : MonoBehaviour, IGUIMeshProvider {

        public RectTransform rectTransform { get { return transform as RectTransform; } }

        public abstract Vector2[] verticles { get; }

        public abstract Triangle[] triangles { get; }

        public abstract bool isProviderEnabled { get; }

        public abstract Vector2 originalBounds { get; }

        public abstract float leftSlicedBound { get; }

        public abstract float rightSlicedBound { get; }

        public abstract float topSlicedBound { get; }

        public abstract float bottomSlicedBound { get; }

        public abstract Vector2[] outerCircuit { get; }

        public abstract Vector2 GetUV(int index);

        //public abstract UIVertex ProcessVertex(UIVertex vertex, int index);

        public abstract void Calculate(bool callRefresh);
    }
}