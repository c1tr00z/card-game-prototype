using c1tr00z.CardPrototype.Characters;
using c1tr00z.CardPrototype.Characters.Deck;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUIPlayerDeckControllerView : MonoBehaviour {

        private PlayerDeckController _playerDeckController;

        [SerializeField]
        private UIList _cardsList;

        [SerializeField]
        private Text _cardsInDeckText;

        [SerializeField]
        private Text _cardsInDiscardText;

        IEnumerator Start() {
            while (BattleController.instance == null
                || BattleController.instance.characters == null
                || BattleController.instance.characters.Count == 0) {

                yield return 0;
            }

            _playerDeckController = BattleController.instance.characters.SelectNotNull(c => c as PlayerCharacter).First().playerDeckController;
            _playerDeckController.changed += UpdateView;
            UpdateView();
        }

        private void UpdateView() {
            _cardsList.UpdateList(_playerDeckController.cardInHand);
            _cardsInDeckText.text = _playerDeckController.cardInDeck.Count().ToString();
            _cardsInDiscardText.text = _playerDeckController.cardInDiscard.Count().ToString();
        }
    }
}