using UnityEngine;

namespace Cards
{
    public abstract class Card : ScriptableObject
    {
        [SerializeField] private string _id;
        private CardType _type;

        public string Id => _id;
        public CardType Type => _type;

        protected Card(CardType type)
        {
            _type = type;
        }
    }

    public enum CardType
    {
        Army,
        HeroEquip,
        ArmyEquip,
        Situation,
        Enemy
    }
}