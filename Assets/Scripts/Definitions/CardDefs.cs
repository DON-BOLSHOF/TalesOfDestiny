using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Definitions
{
    public abstract class CardDefs<T> : ScriptableObject where T:LevelCard
    {
        [SerializeField] private List<T> _cards;

        public int CardsCount => _cards.Count;

        public T Get(string id)
        {
            if (id == null)
                return default;
            
            foreach (var itemDef in _cards)
            {
                if (itemDef.Id == id)
                {
                    return itemDef;
                }
            }

            return default;
        }

        public T Get(int id)
        {
            return _cards[id];
        }
    }
}