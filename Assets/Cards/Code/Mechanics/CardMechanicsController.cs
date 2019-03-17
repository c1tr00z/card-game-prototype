using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.CardPrototype.Cards.Mechanics {
    public class CardMechanicsController : MonoBehaviour {

        private List<CardMechanicsBase> _mechanicsList;

        public bool inited { get; private set; }

        private void Start() {
            _mechanicsList = new List<CardMechanicsBase>();
            DB.GetAll<CardMechanicsDBEntry>().ForEach(dbEntry => {
                var mechanics = dbEntry.LoadPrefab<CardMechanicsBase>().Clone();
                mechanics.transform.Reset(transform);
                _mechanicsList.Add(mechanics);
            });

            inited = true;
        }

        public CardMechanicsBase GetCardMechanics(CardMechanicsDBEntry mechanicsDBEntry) {
            return _mechanicsList.Where(m => m.dbEntry == mechanicsDBEntry).First();
        }
    }
}