using c1tr00z.CardPrototype.Cards.Mechanics;
using c1tr00z.CardPrototype.Characters;

namespace c1tr00z.CardPrototype.Cards {
    public class Card {

        public static event System.Action<Card> cardStateChanged;

        public static event System.Action<Card, CharacterBase> cardPlayed;

        public enum State {
            Deck,
            Hand,
            Discard,
        }

        public CardDBEntry dbEntry { get; private set; }
        public State state { get; private set; }

        public Card(CardDBEntry parent) {
            dbEntry = parent;
            Shuffle();
        }

        public void Draw() {
            ChangeState(State.Hand);
        }

        public void Shuffle() {
            ChangeState(State.Deck); 
        }

        public void Play(CharacterBase owner) {
            dbEntry.mechanics.ForEach(m => App.instance.Get<CardMechanicsController>().GetCardMechanics(m.mechanic).Play(owner, m.param));
            cardPlayed.SafeInvoke(this, owner);
            ChangeState(State.Discard);
        }

        public void Discard() {
            ChangeState(State.Discard);
        }

        private void ChangeState(State newState) {
            state = newState;
            cardStateChanged.SafeInvoke(this);
        }
    }
}
