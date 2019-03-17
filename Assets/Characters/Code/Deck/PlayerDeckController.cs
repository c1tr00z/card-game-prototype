using c1tr00z.CardPrototype.Cards;
using System.Collections.Generic;

namespace c1tr00z.CardPrototype.Characters.Deck {
    public class PlayerDeckController : CharacterDeckController {

        public event System.Action changed;
        
        public PlayerDeckDBEntry playerDeckEntry {
            get { return deckDBEntry as PlayerDeckDBEntry; }
        }

        public List<Card> allPlayerCards { get; private set; }

        public IEnumerable<Card> cardInDeck {
            get { return allPlayerCards != null ? allPlayerCards.Where(c => c.state == Card.State.Deck) : new List<Card>(); }
        }

        public IEnumerable<Card> cardInHand {
            get { return allPlayerCards != null ? allPlayerCards.Where(c => c.state == Card.State.Hand) : new List<Card>(); }
        }

        public IEnumerable<Card> cardInDiscard {
            get { return allPlayerCards != null ? allPlayerCards.Where(c => c.state == Card.State.Discard) : new List<Card>(); }
        }

        private void OnEnable() {
            Card.cardStateChanged += Card_cardStateChanged;
        }

        private void OnDisable() {
            Card.cardStateChanged -= Card_cardStateChanged;
        }

        private void Card_cardStateChanged(Card card) {
            if (allPlayerCards.Contains(card)) {
                changed.SafeInvoke();
            }
        }

        public override void Init() {
            allPlayerCards = new List<Card>();

            playerDeckEntry.cards.ForEach(cardDBEntry => {
                allPlayerCards.Add(new Card(cardDBEntry));
            });

            changed.SafeInvoke();
        }

        public void DiscardAll() {
            var allInHand = cardInHand;
            allInHand.ForEach(c => c.Discard());
            changed.SafeInvoke();
        }

        public override Card DrawCard() {
            if (cardInDeck.Count() == 0) {
                cardInDiscard.ForEach(c => c.Shuffle());
            }
            var card = cardInDeck.RandomItem();
            card.Draw();

            changed.SafeInvoke();

            return card;
        }

        public void DrawCards(int count) {
            for (var i = 0; i < count; i++) {
                DrawCard();
            }
        }
    }
}