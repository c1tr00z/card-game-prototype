using c1tr00z.CardPrototype.Cards;
using c1tr00z.CardPrototype.Characters.Config;

namespace c1tr00z.CardPrototype.Characters {
    public class OpponentCharacter : CharacterBase {

        public Card currentAction { get; private set; }
        
        public OpponentCharacterConfig enemyConfig {
            get { return config as OpponentCharacterConfig; }
        }

        public int actionsCount { get; private set; }

        protected override bool Pay(Card card) {
            return true;
        }

        public override void Play(Card card) {
            if (Pay(card)) {
                currentAction = card;
            }
            OnChanged();
        }

        public void ExecuteAction() {
            if (currentAction != null) {
                currentAction.Play(this);
            }
            OnChanged();
        }

        public void PrepareCard() {
            Play(deckController.DrawCard());
            OnChanged();
        }
    }
}