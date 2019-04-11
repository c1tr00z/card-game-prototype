﻿using c1tr00z.AssistLib.Localization;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    public static class CardEditorHorizontalView {

        public static void DrawCardGUI(CardDBEntry cardDBEntry) {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(cardDBEntry.name, GUILayout.Width(CardsEditorWindow.SELECTOR_WIDTH))) {
                Selection.activeObject = cardDBEntry;
            }
            GUILayout.Label("None", GUILayout.Width(CardsEditorWindow.ICON_WIDTH));
            GUILayout.Label(cardDBEntry.GetTitle(), GUILayout.Width(CardsEditorWindow.NAME_WIDTH));
            cardDBEntry.energyPrice = EditorGUILayout.IntField(cardDBEntry.energyPrice, GUILayout.Width(CardsEditorWindow.ENERGY_PRICE_WIDTH));
            DrawMechanicsBlock(cardDBEntry);
            EditorGUILayout.EndHorizontal();

            if (UnityEngine.GUI.changed) {
                EditorUtility.SetDirty(cardDBEntry);
            }
        }

        public static void DrawMechanicsBlock(CardDBEntry cardDBEntry) {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("+", GUILayout.Width(50))) {
                var mechanicsList = cardDBEntry.mechanics.ToList();
                mechanicsList.Add(new Mechanics.CardMechanicsEntry());
                cardDBEntry.mechanics = mechanicsList.ToArray();
            }
            cardDBEntry.mechanics.ForEach(m => CardMechanicEditorView.DrawGUI(cardDBEntry, m));
            EditorGUILayout.EndVertical();
        }
    }
}