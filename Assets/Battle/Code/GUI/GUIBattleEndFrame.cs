using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIBattleEndFrame : MonoBehaviour {

        [SerializeField]
        private Text _winnerText;

        private void Start() {

            var winner = BattleController.instance.characters.Where(c => !c.died).First();

            _winnerText.text = string.Format("<color=#{0}>{1}</color> is Win!", ColorUtility.ToHtmlStringRGB(winner.dbEntry.color), winner.dbEntry.characterPlayerName);
        }

        public void Finish() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}