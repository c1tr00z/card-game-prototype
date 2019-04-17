using c1tr00z.AssistLib.Localization;
using c1tr00z.CardPrototype.Cards;
using c1tr00z.CardPrototype.Characters;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIBattleFrame : MonoBehaviour {

        [SerializeField]
        private Text _currentPlayerText;

        [SerializeField]
        private Text _lastActionText;

        private void OnEnable() {
            TurnController.characterChanged += TurnController_characterChanged;
            Card.cardPlayed += Card_cardPlayed;
        }

        private void OnDisable() {
            Battle.TurnController.characterChanged -= TurnController_characterChanged;
        }

        private IEnumerator Start() {

            while (BattleController.instance == null
                || BattleController.instance.characters == null
                || BattleController.instance.characters.Count == 0) {
                yield return null;
            }

            BattleController.instance.characters.ForEach(c => {

                var characterViewItem = 
                    DB.GetAll<GUICharacterViewDBEntry>().Where(viewItem => viewItem.character == c.dbEntry).First();
                
                if (characterViewItem == null) {
                    return;
                }

                var view = characterViewItem.LoadPrefab<GUICharacterView>().Clone();
                view.transform.Reset(transform);
                view.Init(c);
            });
        }

        private void TurnController_characterChanged(BaseCharacterTurnController obj) {
            _currentPlayerText.text = obj.character.dbEntry.characterPlayerName;
            _currentPlayerText.color = obj.character.dbEntry.color;
        }

        private void Card_cardPlayed(Card card, CharacterBase character) {
            var cardTextString = "";
            card.dbEntry.mechanics.ForEach(m => cardTextString += (m.description + ";\r\n"));
            _lastActionText.text = string.Format("<color=#{0}>{1}</color> played: {2}\\\\ \r\n {3}", 
                ColorUtility.ToHtmlStringRGB(character.dbEntry.color),
                character.dbEntry.characterPlayerName, card.dbEntry.GetTitle(), cardTextString);
        }

        public void EndTurn() {
            TurnController.instance.turnControllers.Select(tc => tc as PlayerTurnController).First().EndTurn();
            Debug.LogError((transform as RectTransform).rect);
        }
    }
}
