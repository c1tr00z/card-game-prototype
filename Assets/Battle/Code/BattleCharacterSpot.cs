using c1tr00z.CardPrototype.Characters;
using UnityEngine;

namespace c1tr00z.CardPrototype.Battle {
    public class BattleCharacterSpot : MonoBehaviour {

        [SerializeField]
        private CharacterDBEntry _characterDBEntry;

        public CharacterDBEntry characterDBEntry {
            get { return _characterDBEntry; }
        }
    }
}
