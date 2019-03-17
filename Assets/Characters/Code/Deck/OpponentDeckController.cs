using c1tr00z.CardPrototype.Cards;
using UnityEngine;

namespace c1tr00z.CardPrototype.Characters.Deck {
    public class OpponentDeckController : CharacterDeckController {

        public OpponentDeckDBEntry opponentDBEntry {
            get { return deckDBEntry as OpponentDeckDBEntry; }
        }

        public override void Init() { }

        public override Card DrawCard() {
            var chance = Random.Range(0f, 1f);
            var card = new Card(opponentDBEntry.cardChances.Where(cardChance => chance <= cardChance.chance).RandomItem().card);
            return card;
        }
    }
}
