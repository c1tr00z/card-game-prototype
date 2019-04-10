using c1tr00z.CardPrototype.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.CardPrototype.Battle.GUI {
    public class GUICharacterView : MonoBehaviour {

        public Vector2 offset;

        protected CharacterBase character { get; private set; }

        public RectTransform rectTransform {
            get { return transform as RectTransform; }
        }

        [SerializeField]
        private Slider _healthSlider;

        [SerializeField]
        private Text _healthText;

        [SerializeField]
        private Text _armorText;

        public int maxHealth {
            get { return character.initHP; }
        }

        public int currrentHealth {
            get { return character.hp; }
        }

        public float healthValue {
            get { return currrentHealth * 1f / maxHealth; }
        }

        public void Init(CharacterBase character) {
            this.character = character;
            character.changed += UpdateView;

            rectTransform.anchorMin = Vector3.zero;
            rectTransform.anchorMax = Vector3.zero;

            Follow();
        }

        private void OnEnable() {
            Follow();
        }

        public void Start() {
            Follow();
        }

        protected void Follow() {
            if (character == null) {
                return;
            }
            var camera = Camera.allCameras.Where(c => (c.cullingMask & (1 << 5)) == 0).First();

            var cameraPosition = camera.WorldToScreenPoint(character.transform.position).ToVector2();
            var scale = GetComponentInParent<CanvasScaler>().transform.localScale.ToVector2();

            rectTransform.anchoredPosition = new Vector2(cameraPosition.x / scale.x, cameraPosition.y / scale.y) + offset;

            UpdateView();
        }

        protected virtual void UpdateView() {
            if (character == null) {
                return;
            }

            _healthSlider.value = healthValue;
            _healthText.text = string.Format("{0}/{1}", currrentHealth, maxHealth);
            _armorText.text = character.armor.ToString();
        }
    }
}