using c1tr00z.CardPrototype.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Characters.Deck {
    public class OpponentDeckDBEntry : BaseCharacterDeckDBEntry {

        [System.Serializable]
        public class CardChance {
            public CardDBEntry card;
            public float chance;
        }

        public CardChance[] cardChances;
    }
}
