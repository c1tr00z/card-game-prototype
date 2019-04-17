using c1tr00z.AssistLib.Localization;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    public class CardMechanicsDBEntry : DBEntry {
        public enum Target {
            Self,
            Opponent
        }

        public Target target;

        public Color color = Color.white;

        public string GetDescription(object param) {
            return this.GetLocalizationText("Description", param);
        }
    }
}
