using c1tr00z.CardPrototype.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Battle {
    public class OpponentTurnController : BaseCharacterTurnController {


        public OpponentCharacter opponentCharacter {
            get {
                return character as OpponentCharacter;
            }
        }

        public override IEnumerator C_Turn() {
            yield return null;
            opponentCharacter.PrepareCard();
        }

        public override void PostTurnActions() { }

        public override void PreTurnActions() {
            base.PreTurnActions();
            opponentCharacter.ExecuteAction();
        }
    }
}