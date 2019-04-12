using c1tr00z.CardPrototype.Characters;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    public class CardMechanicHeal : CardMechanicsBase {
        public override void Play(CharacterBase owner, int param) {
            owner.Heal(param);
        }
    }
}