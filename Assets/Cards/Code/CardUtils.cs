using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    public static class CardUtils {
        
        public static Color GetColor(this Card card) {
            return card.dbEntry.mechanics.MaxElement(m => m.param).mechanic.color;
        }

        public static string GetText(this Card card) {
            var cardTextString = "";
            card.dbEntry.mechanics.ForEach(m => cardTextString += (m.description + ";\r\n"));
            return cardTextString.Trim();
        }
    }
}
