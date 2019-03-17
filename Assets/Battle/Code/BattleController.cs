using c1tr00z.CardPrototype.Cards.Mechanics;
using c1tr00z.CardPrototype.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Battle {
    public class BattleController : BehaviourSingleton<BattleController> {

        public static event System.Action CharactersInited;

        [SerializeField]
        private BattleCharacterSpot[] _charactersSpots;

        [SerializeField]
        private UIFrameItem _battleFrame;

        [SerializeField]
        private UIFrameItem _winnerFrame;
        
        public List<CharacterBase> characters { get; private set; }

        private void OnEnable() {
            CharacterBase.characterDied += CharacterBase_characterDied;
        }

        private void OnDisable() {
            CharacterBase.characterDied -= CharacterBase_characterDied;
        }

        private IEnumerator Start() {
            while (App.instance.Get<CardMechanicsController>() == null
                || !App.instance.Get<CardMechanicsController>().inited) {
                yield return null;
            }
            InitCharacters();
            UI.instance.Show(_battleFrame);
        }

        private void InitCharacters() {
            characters = _charactersSpots.Select(spot => {
                var character = spot.characterDBEntry.LoadPrefab<CharacterBase>().Clone();
                character.Reset(spot.transform);
                character.Init();
                return character;
            }).ToList();
        }

        public CharacterBase GetOpponent(CharacterBase character) {
            return characters.Where(c => c != character).First();
        }

        private void CharacterBase_characterDied(CharacterBase character) {
            UI.instance.Show(_winnerFrame);
        }
    }
}
