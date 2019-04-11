using c1tr00z.AssistLib.Localization;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    [CustomEditor(typeof(CardDBEntry))]
    public class CardDBEntryInspector : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            var cardDBEntry = target as CardDBEntry;

            if (cardDBEntry == null) {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ", EditorStyles.boldLabel);
            GUILayout.Label(cardDBEntry.GetTitle());
            EditorGUILayout.EndHorizontal();
            cardDBEntry.energyPrice = EditorGUILayout.IntField("Energy price", cardDBEntry.energyPrice);
            CardEditorHorizontalView.DrawMechanicsBlock(cardDBEntry);

            if (UnityEngine.GUI.changed) {
                EditorUtility.SetDirty(cardDBEntry);
            }
        }
    }
}