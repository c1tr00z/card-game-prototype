using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.UI {
    public class UISectoralLayoutGroup : UIRadialLayoutGroup {

        public int sectorAngle = 30;

        public Vector2 positionDelta;

        protected override void Refresh() {
            var children = GetChildren();

            var count = children.Count;

            var firstAngle = (children.Count - 1) * (-1 * step / 2);

            for (var i = 0; i < children.Count; i++) {
                var child = children[i];
                var angle = firstAngle + (i * step);
                SetPositionByAngleAndRadius(child, angle, true);

                child.anchoredPosition += positionDelta;
            }
        }
    }
}