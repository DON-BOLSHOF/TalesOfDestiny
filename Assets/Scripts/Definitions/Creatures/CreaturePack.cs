using System;
using UnityEngine;

namespace Definitions.Creatures
{
    [Serializable]
    public abstract class CreaturePack
    {
        [SerializeField] private int _count;

        [SerializeField]protected CreatureCard creatureCard;
        public CreatureCard CreatureCard => creatureCard;
        public int Count => _count;
        
        protected CreaturePack(){}

        protected CreaturePack(CreatureCard card, int count)
        {
            creatureCard = card;
            _count = count;
        }

        protected CreaturePack(CreaturePack pack)
        {
            creatureCard = pack.creatureCard;
            _count = pack._count;
        }

        public void ModifyCount(int value)
        {
            _count += value;
        }

        public override string ToString()
        {
            return $"Creature: {creatureCard.Id} has {_count} amount";
        }
    }
}