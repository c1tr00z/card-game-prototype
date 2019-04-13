using c1tr00z.AssistLib.GoogleSpreadsheetImporter;
using c1tr00z.CardPrototype.Cards.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    [EditorToolName("Import card balance")]
    public class CardBalanceImporterEditorTool : GoogleSpreadsheetDocumentImportEditorTool {

        private static readonly string CARD_NAME = "Name";
        private static readonly string CARD_ENERGY_PRICE = "Energy";
        private static readonly string CARD_MECHANICS = "Mechanics";

        protected override void ProcessImport() {
            var allCards = DB.GetAll<CardDBEntry>();
            var allMechanics = DB.GetAll<CardMechanicsDBEntry>();
            pages.ForEach(p => {
                var imports = GoogleSpreadsheetDocumentImpoter.Import(p);
                imports.Keys.ForEach(importedField => {
                    imports[importedField].ForEach(value => {
                        var card = allCards.Where(c => c.name == value.Key).First();
                        if (card == null) {
                            card = CardDBEntryUtils.CreateCardDBEntry(value.Key);
                        }
                        if (importedField == CARD_ENERGY_PRICE) {
                            card.energyPrice = int.Parse(value.Value);
                        } else if (importedField == CARD_MECHANICS) {
                            var mechanics = new List<CardMechanicsEntry>();
                            value.Value.Split('|').ForEach(mechanicString => {
                                var mvs = mechanicString.Split(':');
                                mechanics.Add(new CardMechanicsEntry { mechanic = allMechanics.Where(m => m.name == mvs[0]).First(), param = int.Parse(mvs[1]) });
                            });
                            card.mechanics = mechanics.ToArray();
                        }
                        EditorUtility.SetDirty(card);
                    });
                });
            });
        }
    }
}