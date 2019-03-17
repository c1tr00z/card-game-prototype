using c1tr00z.CardPrototype.Cards.Mechanics;

namespace c1tr00z.CardPrototype.Cards {
    public class CardDBEntry : DBEntry {

        public string cardName;

        public int energyPrice;

        public CardMechanicsEntry[] mechanics;
    }
}