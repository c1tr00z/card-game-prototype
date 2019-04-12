using c1tr00z.CardPrototype.Cards;
using c1tr00z.CardPrototype.Characters.Config;
using c1tr00z.CardPrototype.Characters.Deck;
using UnityEngine;

namespace c1tr00z.CardPrototype.Characters {
    public abstract class CharacterBase : MonoBehaviour {

        public event System.Action changed;

        public static event System.Action<CharacterBase> characterDied;

        private CharacterDBEntry _dbEntry;

        protected CharacterConfigBase config { get; private set; }

        protected CharacterDeckController deckController { get; set; }

        public bool died { get; private set; }

        public CharacterDBEntry dbEntry {
            get {
                if (_dbEntry == null) {
                    DBEntryResource resource;
                    if (this.TryGetComponent(out resource)) {
                        _dbEntry = resource.parent as CharacterDBEntry;
                    }
                }
                return _dbEntry;
            }
        }

        public int initHP {
            get { return config.initHP; }
        }
        public int hp { get; private set; }
        public int armor { get; private set; }

        protected abstract bool Pay(Card card);

        public void Init() {
            Init(dbEntry.Load<CharacterConfigBase>("Config"), dbEntry.Load<BaseCharacterDeckDBEntry>("Deck"));
        }

        public virtual void Init(CharacterConfigBase config, BaseCharacterDeckDBEntry deckConfig) {
            this.config = config;
            deckController = deckConfig.LoadPrefab<CharacterDeckController>().Clone();
            deckController.transform.SetParent(transform);
            hp = initHP;
            OnChanged(); 
        }

        public void ClearArmor() {
            armor = 0;
            OnChanged();
        }

        public virtual void Play(Card card) {
            if (Pay(card)) {
                card.Play(this);
            }
            OnChanged();
        }

        public void AddArmor(int value) {
            armor += value;
            OnChanged();
        }

        public void Damage(int value) {
            if (armor > 0) { 
                var diff = armor - value;
                value = diff < 0 ? Mathf.Abs(diff) : 0;
                armor = diff > 0 ? diff : 0;
            }
            hp -= value;
            OnChanged();
            if (hp <= 0) {
                died = true;
                characterDied.SafeInvoke(this);
            }
        }

        public void Heal(int value) {
            hp = hp + value <= initHP ? hp + value : initHP;
            OnChanged();
        }

        protected void DoActionAndSetChanged(System.Action action) {
            action.SafeInvoke();
            OnChanged();
        }

        protected void OnChanged() {
            changed.SafeInvoke();
        }
    }
}