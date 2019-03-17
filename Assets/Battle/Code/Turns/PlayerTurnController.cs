using c1tr00z.CardPrototype.Characters;
using System.Collections;

namespace c1tr00z.CardPrototype.Battle {
    public class PlayerTurnController : BaseCharacterTurnController {

        public PlayerCharacter playerCharacter {
            get {
                return character as PlayerCharacter;
            }
        }
        
        public bool turnInProgress { get; private set; }

        public override IEnumerator C_Turn() {
            turnInProgress = true;

            while (turnInProgress) {
                yield return 0;
            }

        }

        protected override void OnCharacterChanged() {
            base.OnCharacterChanged();
            if (!isActive) {
                return;
            }
            if (turnInProgress && playerCharacter.currentEnergy <= 0) {
                turnInProgress = false;
            }
        }

        public void EndTurn() {
            if (!isActive) {
                return;
            }
            turnInProgress = false;
        }

        public override void PostTurnActions() {
            playerCharacter.DiscardCards();
        }

        public override void PreTurnActions() {
            playerCharacter.ClearEnergy();
            base.PreTurnActions();
            playerCharacter.RefreshEnergy();
            playerCharacter.DrawCards();
        }
    }
}