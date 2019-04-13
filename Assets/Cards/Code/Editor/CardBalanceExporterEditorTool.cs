using c1tr00z.AssistLib.EditorTools;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    [EditorToolName("Export card balance")]
    public class CardBalanceExporterEditorTool : EditorTool {

        protected override void DrawInterface() {
            if (Button("Export")) {
                var allCardsDBEntries = DB.GetAll<CardDBEntry>();
                var allCardsBalanceText = string.Join("\r\n",
                allCardsDBEntries.Select(c => string.Format("\"{0}\",\"{1}\",\"{2}\"", c.name, c.energyPrice, string.Join("|", c.mechanics.Select(m => string.Format("{0}:{1}", m.mechanic.name, m.param)).ToArray()))).ToArray());
                Debug.Log(allCardsBalanceText);

                FileUtils.SaveTextToFile(PathUtils.Combine(Application.dataPath, "Export", "cardBalanceText.csv"), allCardsBalanceText);
            }
        }
    }
}