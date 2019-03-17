using c1tr00z.CardPrototype.Characters;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    public class CardMechanicsAttack : CardMechanicsBase {
        public override void Play(CharacterBase owner, int param) {
            Battle.BattleController.instance.GetOpponent(owner).Damage(param); 
        }
    }
}
