using c1tr00z.CardPrototype.Cards.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Cards.GUI {
    public class GUIMechanicView : MonoBehaviour, IUIListItemView {

        public CardMechanicsEntry mechanicsEntry { get; private set; }

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Text _value;

        public void UpdateItem(object item) {

            mechanicsEntry = item as CardMechanicsEntry;

            UpdateView();
        }

        private void UpdateView() {
            _icon.sprite = mechanicsEntry.mechanic.Load<UISpriteItem>("Icon").sprite;
            _value.text = mechanicsEntry.param.ToString();
        }
    }
}