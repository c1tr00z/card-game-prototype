using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    [System.Serializable]
    public class CardMechanicsEntry {
        public CardMechanicsDBEntry mechanic;
        public int param;

        public string description {
            get { return mechanic.GetDescription(param); }
        }
    }
}
