using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Other {
    public class GameRulesDBEntry : DBEntry {
        
        public TextAsset LoadRulesText() {
            return this.Load<TextAsset>(AssistLib.Localization.Localization.currentLanguage.ToString());
        }
    }
}