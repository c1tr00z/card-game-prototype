using c1tr00z.CardPrototype.Cards;
using System.Collections.Generic;

namespace c1tr00z.CardPrototype.Battle {
    public class BattleLog : BehaviourSingleton<BattleLog> {

        public static event System.Action logUpdated;

        private List<BattleLogEntry> _entries = new List<BattleLogEntry>();

        public List<BattleLogEntry> entries {
            get {
                return _entries;
            }
        }

        //public List<LogEntry>

        private void OnEnable() {
            Card.cardPlayed += Card_cardPlayed;
        }

        private void OnDisable() {
            Card.cardPlayed -= Card_cardPlayed;
        }

        private void Card_cardPlayed(Card card, Characters.CharacterBase character) {
            entries.Add(new BattleLogEntry {
                card = card,
                character = character
            });
            logUpdated.SafeInvoke();
        }
    }
}