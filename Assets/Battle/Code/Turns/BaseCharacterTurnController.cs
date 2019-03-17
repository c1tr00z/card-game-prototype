using c1tr00z.CardPrototype.Characters;
using System.Collections;
using UnityEngine;

namespace c1tr00z.CardPrototype.Battle {
    public abstract class BaseCharacterTurnController : MonoBehaviour {

        public CharacterBase character { get; private set; }

        public bool isActive {
            get { return TurnController.instance.IsActive(this); }
        }
        
        public void Init(CharacterBase character) {
            this.character = character;
            character.changed += OnCharacterChanged;
        }

        public virtual void PreTurnActions() {
            character.ClearArmor();
        }

        protected virtual void OnCharacterChanged() { }

        public abstract IEnumerator C_Turn();

        public abstract void PostTurnActions();
    }
}