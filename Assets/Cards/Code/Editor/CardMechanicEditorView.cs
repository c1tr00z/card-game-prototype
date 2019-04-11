using c1tr00z.CardPrototype.Cards.Mechanics;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    public static class CardMechanicEditorView {

        private static readonly int REMOVE_WIDTH = 20;
        private static readonly int PARAM_WIDTH = 30;

        private static List<CardMechanicsDBEntry> _mechanics = new List<CardMechanicsDBEntry>();
        
        public static void DrawGUI(CardDBEntry card, CardMechanicsEntry mechanicEntry) {

            if (_mechanics.Count == 0) {
                _mechanics.AddRange(DB.GetAll<CardMechanicsDBEntry>());
            }

            EditorGUILayout.BeginHorizontal();
            mechanicEntry.mechanic = mechanicEntry.mechanic == null ? _mechanics.First() : mechanicEntry.mechanic;
            mechanicEntry.mechanic = _mechanics[EditorGUILayout.Popup(_mechanics.IndexOf(mechanicEntry.mechanic), _mechanics.Select(m => m.name).ToArray())];
            mechanicEntry.param = EditorGUILayout.IntField(mechanicEntry.param, GUILayout.Width(PARAM_WIDTH));
            if (GUILayout.Button("-", GUILayout.Width(REMOVE_WIDTH))) {
                var cardMechanicsList = card.mechanics.ToList();
                cardMechanicsList.Remove(mechanicEntry);
                card.mechanics = cardMechanicsList.ToArray();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}