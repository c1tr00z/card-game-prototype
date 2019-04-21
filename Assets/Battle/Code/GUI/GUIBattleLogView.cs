using System.Collections;
using UnityEngine;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIBattleLogView : MonoBehaviour {

        [SerializeField] private UIList _list;

        [SerializeField] private GameObject _logView;

        [SerializeField] private GUIBattleLogEntryView _lastActionView;

        private void OnEnable() {
            BattleLog.logUpdated += BattleLog_logUpdated;
        }

        private void OnDisable() {
            BattleLog.logUpdated -= BattleLog_logUpdated;
        }

        IEnumerator Start() {

            _logView.SetActive(false);

            while (BattleLog.instance == null) {
                yield return null;
            }

            BattleLog_logUpdated();
        }

        private void BattleLog_logUpdated() {
            _list.UpdateList(BattleLog.instance.entries);
            _lastActionView.UpdateItem(BattleLog.instance.entries.Last());
        }

        public void ShowLog() {
            _logView.SetActive(!_logView.activeSelf);
        }
    }
}