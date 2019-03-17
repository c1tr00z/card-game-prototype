using c1tr00z.CardPrototype.Characters;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    public class CardMechanicsDefence : CardMechanicsBase {

        public override void Play(CharacterBase owner, int param) {
            owner.AddArmor(param);
        }
    }
}