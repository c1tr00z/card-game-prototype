using c1tr00z.AssistLib.Localization;
using c1tr00z.CardPrototype.Cards;
using UnityEngine;

namespace c1tr00z.CardPrototype.Localization {
    public static class LocalizationKeys {

        public static string CARD_PLAYED = "CardPlayed";

        public static string CardPlayedString(Card card) {
            return string.Format(AssistLib.Localization.Localization.Translate(CARD_PLAYED), ColorUtility.ToHtmlStringRGB(card.GetColor()), card.dbEntry.GetTitle(), card.GetText());
        }
    }
}