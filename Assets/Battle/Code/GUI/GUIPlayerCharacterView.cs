using c1tr00z.CardPrototype.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIPlayerCharacterView : GUICharacterView {

        public PlayerCharacter playerCharacter {
            get { return character as PlayerCharacter; }
        }

        [SerializeField]
        private Slider _energySlider;

        [SerializeField]
        private Text _energyText;

        protected override void UpdateView() {
            base.UpdateView();

            _energySlider.value = playerCharacter.currentEnergy * 1f / playerCharacter.initEnergy;
            _energyText.text = string.Format("{0}/{1}", playerCharacter.currentEnergy, playerCharacter.initEnergy);
        }
    }
}
