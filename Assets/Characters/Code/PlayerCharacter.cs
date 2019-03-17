using c1tr00z.CardPrototype.Cards;
using c1tr00z.CardPrototype.Characters.Config;
using c1tr00z.CardPrototype.Characters.Deck;

namespace c1tr00z.CardPrototype.Characters {
    public class PlayerCharacter : CharacterBase {

        public PlayerCharacterConfig playerConfig {
            get { return config as PlayerCharacterConfig; }
        }

        public int initEnergy {
            get { return playerConfig.initEnergy; }
        }
        public int currentEnergy { get; private set; }

        public PlayerDeckController playerDeckController {
            get { return deckController as PlayerDeckController; }
        }

        public void ClearEnergy() {
            currentEnergy = 0;
            OnChanged();
        }

        public void RefreshEnergy() {
            currentEnergy = initEnergy;
            OnChanged();
        }

        protected override bool Pay(Card card) {
            if (currentEnergy >= card.dbEntry.energyPrice) {
                currentEnergy -= card.dbEntry.energyPrice;
                OnChanged();
                return true;
            }
            OnChanged();
            return false;
        }

        public void DrawCards() {
            playerDeckController.DrawCards(playerConfig.cardsInHand);
            OnChanged();
        }

        public void DiscardCards() {
            playerDeckController.DiscardAll();
            OnChanged();
        }
    }
}
