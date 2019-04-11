using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    public class CardsEditorController {

        public static event System.Action cardAdded;
        public static event System.Action cardRemoved;

        private List<CardDBEntry> _cards;

        public List<CardDBEntry> cards {
            get { return _cards; }
        }

        public CardsEditorController() {
            _cards = DB.GetAll<CardDBEntry>().SelectNotNull().ToList();
            _cards.Sort((c1, c2) => string.Compare(c1.name, c2.name));
        }

        public void CreateNewCard(string newCardName) {
            var path = PathUtils.Combine("Assets", "Cards", "Resources", "Cards");
            var trueCardName = newCardName.Replace(" ", "");
            var newCard = AssetDBUtils.CreateScriptableObject<CardDBEntry>(path, trueCardName);
            AddNewCard(newCard);
            var newCardIcon = AssetDBUtils.CreateScriptableObject<UISpriteItem>(path, string.Format("{0}@Icon", trueCardName));
            ItemsEditor.CollectItems();
        }

        public void AddNewCard(CardDBEntry newCard) {
            _cards.Add(newCard);
            _cards = _cards.SelectNotNull().ToList();
            _cards.Sort((c1, c2) => string.Compare(c1.name, c2.name));
            cardAdded.SafeInvoke();
        }

        public void RemoveCard(CardDBEntry cardDBEntry) {
            _cards.Remove(cardDBEntry);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(cardDBEntry.Load<UISpriteItem>("Icon")));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(cardDBEntry));
            ItemsEditor.CollectItems();
            AssetDatabase.SaveAssets();
            cardRemoved.SafeInvoke();
        }
    }
}