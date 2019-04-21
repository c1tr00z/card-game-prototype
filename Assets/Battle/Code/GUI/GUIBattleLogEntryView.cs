using c1tr00z.AssistLib.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIBattleLogEntryView : MonoBehaviour, IUIListItemView {

        [SerializeField] private Text _characterTitle;
        [SerializeField] private Text _cardText;

        public BattleLogEntry logEntry { get; private set; }

        public void UpdateItem(object item) {
            logEntry = item as BattleLogEntry;

            if (logEntry == null) {
                return;
            }

            UpdateView();
        }

        private void UpdateView() {
            _characterTitle.color = logEntry.character.dbEntry.color;
            _characterTitle.text = logEntry.character.dbEntry.GetTitle();
            _cardText.text = Localization.LocalizationKeys.CardPlayedString(logEntry.card);
        }
    }
}