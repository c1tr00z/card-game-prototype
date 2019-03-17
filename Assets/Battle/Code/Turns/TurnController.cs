using c1tr00z.CardPrototype.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Battle {
    public class TurnController : BehaviourSingleton<TurnController> {

        public static event System.Action<BaseCharacterTurnController> characterChanged;

        [SerializeField]
        private float _delay = 1f;

        [SerializeField]
        private CharacterDBEntry _firstCharacter;

        private BaseCharacterTurnController _currentCharacter;

        public List<BaseCharacterTurnController> turnControllers { get; private set; }

        private void OnEnable() {
            BattleController.CharactersInited += Init;
        }

        private void OnDisable() {
            BattleController.CharactersInited -= Init;
        }

        public bool IsActive(BaseCharacterTurnController turnController) {
            return turnController == _currentCharacter;
        }

        private IEnumerator Start() {
            while (BattleController.instance == null 
                || BattleController.instance.characters == null
                || BattleController.instance.characters.Count == 0) {

                yield return null;
            }

            Init();
        }

        private void Init() {

            turnControllers = new List<BaseCharacterTurnController>();

            BattleController.instance.characters.ForEach(c => {
                var characterTurnController = c.dbEntry.Load<BaseCharacterTurnController>("TC").Clone(transform);
                characterTurnController.Init(c);
                turnControllers.Add(characterTurnController);
            });

            StartTurnCycle();
        }

        private void StartTurnCycle() {
            _currentCharacter = turnControllers.Where(tc => tc.character.dbEntry == _firstCharacter).First();
            StartCoroutine(C_TurnCycle());
        }

        private IEnumerator C_TurnCycle() {
            while (turnControllers.All(tc => !tc.character.died)) {

                characterChanged.SafeInvoke(_currentCharacter);

                yield return new WaitForSeconds(_delay);

                _currentCharacter.PreTurnActions();

                yield return StartCoroutine(_currentCharacter.C_Turn());

                _currentCharacter.PostTurnActions();

                _currentCharacter = turnControllers.Where(tc => tc.character != _currentCharacter.character).First();
            }
        }
    }
}