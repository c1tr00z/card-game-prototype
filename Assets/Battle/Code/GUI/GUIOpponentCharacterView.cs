using c1tr00z.CardPrototype.Characters;
using UnityEngine;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIOpponentCharacterView : GUICharacterView {

        [SerializeField]
        private UIList _nextActionList;

        public OpponentCharacter opponentCharacter {
            get { return character as OpponentCharacter; }
        }

        protected override void UpdateView() {
            base.UpdateView();

            if (opponentCharacter == null) {
                Debug.LogError(character.GetType(), character);
                return;
            }

            if (opponentCharacter.currentAction == null) {
                return;
            }

            _nextActionList.UpdateList(opponentCharacter.currentAction.dbEntry.mechanics);
        }

    }
}
