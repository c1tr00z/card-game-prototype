using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMeshes {

    public interface IGUIMeshProvider {
        
        Vector2[] verticles { get; }

        Triangle[] triangles { get; }

        bool isProviderEnabled { get; }

        Vector2 originalBounds { get; }

        float leftSlicedBound { get; }
        float rightSlicedBound { get; }
        float topSlicedBound { get; }
        float bottomSlicedBound { get; }

        Vector2[] outerCircuit { get; }

        Vector2 GetUV(int index);

        //UIVertex ProcessVertex(UIVertex vertex, int index);

        void Calculate(bool callRefresh);
    }
}
