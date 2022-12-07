﻿using UnityEngine;
using View;

namespace Cards
{
    public abstract class Card : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private CardView _cardView;
        
        private CardType _type;

        public string Id => _id;
        public CardView View => _cardView;
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