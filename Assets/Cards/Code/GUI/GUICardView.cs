using c1tr00z.AssistLib.Localization;
using c1tr00z.CardPrototype.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Cards.GUI {
    public class GUICardView : MonoBehaviour, IUIListItemView {

        [SerializeField]
        private Text _cardNameText;

        [SerializeField]
        private Text _cardPriceText;

        [SerializeField]
        private Image _cardIcon;

        [SerializeField]
        private Text _cardText;

        [SerializeField]
        private Graphic _back;

        private PlayerCharacter _player;

        public PlayerCharacter player {
            get {
                if (_player == null) {
                    _player = Battle.BattleController.instance.characters.SelectNotNull(c => c as PlayerCharacter).First();
                }
                return _player;
            }
        }

        public Card card { get; private set; }

        public void UpdateItem(object item) {
            if (item == null || card == item) {
                return;
            }

            card = item as Card;

            UpdateView();
        }

        private void UpdateView() {
            _cardNameText.text = card.dbEntry.GetTitle();
            _cardPriceText.text = card.dbEntry.energyPrice.ToString();
            var iconItem = card.dbEntry.Load<UISpriteItem>("Icon");
            if (iconItem != null) {
                _cardIcon.sprite = iconItem.sprite;
            }

            var cardTextString = "";
            card.dbEntry.mechanics.ForEach(m => cardTextString += (m.description + ";\r\n"));
            _back.color = card.dbEntry.mechanics.MaxElement(m => m.param).mechanic.color;
            _cardText.text = cardTextString;
        }

        public void Play() {
            player.Play(card);
        }
    }
}