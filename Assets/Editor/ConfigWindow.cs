using c1tr00z.CardPrototype.Cards;
using System.Collections.Generic;
using UnityEditor;

namespace c1tr00z.CardPrototype.Editor {
    public class ConfigWindow : EditorWindow {

        private IEnumerable<CardDBEntry> _cards;
        private bool _showCards = false;

        //[MenuItem("Ellada Games/Config window")]
        //public static void Open() {
        //    ConfigWindow window = (ConfigWindow)EditorWindow.GetWindow(typeof(ConfigWindow));
        //    window.Show();
        //}

        //private void OnGUI() {
            
        //}
    }
}