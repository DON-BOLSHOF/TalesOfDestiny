using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;

namespace Definitions
{
    public abstract class CardDefs<T> : ScriptableObject where T:Card
    {
        [SerializeField] private List<T> _cards;

        public int CardsCount => _cards.Count;

        public T Get(string id)
        {
            if (id == null)
                return default;

            return _cards.FirstOrDefault(itemDef => itemDef.Id == id);
        }

        public T Get(int id)
        {
            return _cards[id];
        }

        public List<T> GetAllCardsDefs()
        {
            return new List<T>(_cards);
        }
    }
}