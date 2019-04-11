using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards {
    public class CardsEditorWindow : EditorWindow {

        public static readonly int ICON_WIDTH = 100;
        public static readonly int NAME_WIDTH = 300;
        public static readonly int ENERGY_PRICE_WIDTH = 20;
        
        public static readonly int ITEMS_ON_PAGE_WIDTH = 300;
        public static readonly int AVERAGE_BUTTON_WIDTH = 100;
        public static readonly int SMALL_BUTTON_WIDTH = 20;
        public static readonly int PAGE_LABEL_WIDTH = 100;

        private CardsEditorController _controller;

        private List<CardDBEntry> _currentCards;

        private bool _usePagination;
        private int _page;
        private int _itemsOnPage = 10;

        private Vector2 _scrollView;

        private string _newCardName;

        [MenuItem("Card  Game/Edit Cards")]
        public static void ShowSettingsWindow() {
            var cardsWindow = (CardsEditorWindow)EditorWindow.GetWindow(typeof(CardsEditorWindow), true);
            cardsWindow.Load();
        }

        private void Load() {
            _controller = new CardsEditorController();
            CardsEditorController.cardAdded += RefreshList;
            CardsEditorController.cardRemoved += RefreshList;
            RefreshList();
        }

        private void OnDestroy() {
            CardsEditorController.cardAdded -= RefreshList;
            CardsEditorController.cardRemoved -= RefreshList;
        }

        void OnGUI() {
            GUILayout.Label("Cards", EditorStyles.boldLabel);

            DrawCreateCard();

            DrawPagination();

            _scrollView = EditorGUILayout.BeginScrollView(_scrollView);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("#", EditorStyles.boldLabel, GUILayout.Width(AVERAGE_BUTTON_WIDTH + SMALL_BUTTON_WIDTH));
            GUILayout.Label("Icon", EditorStyles.boldLabel, GUILayout.Width(ICON_WIDTH));
            GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(NAME_WIDTH));
            GUILayout.Label("Energy price", EditorStyles.boldLabel, GUILayout.Width(ENERGY_PRICE_WIDTH));
            GUILayout.Label("Mechanics", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            _currentCards.ForEach(c => CardEditorHorizontalView.DrawCardGUI(_controller, c));
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }

        private void DrawPagination() {
            _usePagination = GUILayout.Toggle(_usePagination, "Use pagination");

            if (_usePagination) {
                _itemsOnPage = EditorGUILayout.IntField("Items on page", _itemsOnPage, GUILayout.Width(ITEMS_ON_PAGE_WIDTH));
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("<", GUILayout.Width(SMALL_BUTTON_WIDTH))) {
                    _page = _page > 0 ? _page - 1 : _page;
                    RebuildList();
                }
                GUILayout.Label("Page: " + _page, GUILayout.Width(PAGE_LABEL_WIDTH));
                if (GUILayout.Button(">", GUILayout.Width(SMALL_BUTTON_WIDTH))) {
                    var maxPage = _controller.cards.Count / _itemsOnPage + (_controller.cards.Count % _itemsOnPage > 0 ? 1 : 0);
                    _page = _page < maxPage - 1 ? _page + 1 : _page;
                    RebuildList();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawCreateCard() {
            EditorGUILayout.BeginHorizontal();
            _newCardName = EditorGUILayout.TextField("New Card name", _newCardName, GUILayout.Width(NAME_WIDTH));
            if (GUILayout.Button("Create", GUILayout.Width(AVERAGE_BUTTON_WIDTH))) {
                _controller.CreateNewCard(_newCardName);
                _newCardName = "";
            }
            EditorGUILayout.EndHorizontal();
        }

        private void RefreshList() {
            if (!_usePagination) {
                _currentCards = _controller.cards;
                return;
            }
            _page = 0;
            RebuildList();
        }

        private void RebuildList() {
            if (!_usePagination) {
                _currentCards = _controller.cards;
                return;
            }
            var itemsCountOnCurrentPage = _itemsOnPage > _controller.cards.Count() 
                ? _controller.cards.Count
                : Mathf.Min(_itemsOnPage, _controller.cards.Count - _itemsOnPage * _page);
            _currentCards = _controller.cards.GetRange(_page * _itemsOnPage, itemsCountOnCurrentPage);
        }
    }
}
