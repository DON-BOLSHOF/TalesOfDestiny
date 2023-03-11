using System;
using Cards;
using UnityEngine;

namespace Definitions.Creatures
{
    public abstract class CreatureCard : Card
    {
        [SerializeField] private ArmorType _armorType;
        [SerializeField] private AttackType _attackType;

        [SerializeField] private int _attack;
        [SerializeField] private int _health;

        public ArmorType ArmorType => _armorType;
        public AttackType AttackType => _attackType;
        public int Attack => _attack;
        public int Health => _health;

        protected CreatureCard(CardType type) : base(type)
        {
        }
    }


    [Serializable]
    public enum AttackType
    {
        Chopping,
        Pricking,
        Magic
    }

    [Serializable]
    public enum ArmorType
    {
        Light,
        Medium,
        Heavy
    }
}