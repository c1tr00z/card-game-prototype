using c1tr00z.CardPrototype.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    public abstract class CardMechanicsBase : MonoBehaviour {

        private CardMechanicsDBEntry _dbEntry;

        public CardMechanicsDBEntry dbEntry {
            get {
                if (_dbEntry == null) {
                    DBEntryResource resource;
                    if (this.TryGetComponent<DBEntryResource>(out resource)) {
                        _dbEntry = resource.parent as CardMechanicsDBEntry;
                    }
                }
                return _dbEntry;
            }
        }

        public abstract void Play(CharacterBase owner, int param);
    }
}
