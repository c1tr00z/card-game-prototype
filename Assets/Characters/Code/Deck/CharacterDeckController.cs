using c1tr00z.CardPrototype.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Characters.Deck {
    public abstract class CharacterDeckController : MonoBehaviour {

        public CharacterBase character {
            get { return GetComponentInParent<CharacterBase>(); }
        }

        private BaseCharacterDeckDBEntry _deckDBEntry;

        public BaseCharacterDeckDBEntry deckDBEntry {
            get {
                if (_deckDBEntry == null) {
                    DBEntryResource resource;
                    if (this.TryGetComponent(out resource)) {
                        _deckDBEntry = resource.parent as BaseCharacterDeckDBEntry;
                    }
                }
                return _deckDBEntry;
            }
        }

        private void Start() {
            Init();
        }

        public abstract void Init();

        public abstract Card DrawCard();
    }
}