using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMeshes {
    [System.Serializable]
    public class Triangle {
        public int p1;
        public int p2;
        public int p3;

        public override string ToString() {
            return string.Format("[{0}, {1}, {2}]", p1, p2, p3);
        }
    }
}