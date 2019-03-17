using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    public class CardMechanicsDBEntry : DBEntry {
        public enum Target {
            Self,
            Opponent
        }

        public Target target;

        public string mechanicsText;
    }
}
