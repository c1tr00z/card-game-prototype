namespace c1tr00z.CardPrototype.Cards {
    public static class CardDBEntryUtils {

        public static CardDBEntry CreateCardDBEntry(string name) {
            var path = PathUtils.Combine("Assets", "Cards", "Resources", "Cards");
            var trueCardName = name.Replace(" ", "");
            var newCard = AssetDBUtils.CreateScriptableObject<CardDBEntry>(path, trueCardName);
            var newCardIcon = AssetDBUtils.CreateScriptableObject<UISpriteItem>(path, string.Format("{0}@Icon", trueCardName));
            ItemsEditor.CollectItems();
            return newCard;
        }
    }
}
