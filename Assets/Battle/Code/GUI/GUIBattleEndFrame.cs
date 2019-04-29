using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIBattleEndFrame : MonoBehaviour {

        [SerializeField]
        private Text _winnerText;

        private void Start() {

            var winner = BattleController.instance.characters.Where(c => !c.died).First();

            _winnerText.text = Localization.LocalizationKeys.WinLine(winner);
        }

        public void Finish() {
        }
    }
}