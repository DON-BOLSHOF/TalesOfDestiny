using System;
using Cards;
using UnityEngine;

namespace Definitions.Enemies
{
    [CreateAssetMenu(menuName = "Defs/Enemy", fileName = "Enemy")]
    public class EnemyCard : Card
    {
        [SerializeField] private ArmorType _armorType;
        [SerializeField] private AttackType _attackType;

        [SerializeField] private int _attack;
        [SerializeField] private int _health;
        
        [SerializeField] private int _turnThreshold;
        
        public ArmorType ArmorType => _armorType;
        public AttackType AttackType => _attackType;
        public int Attack => _attack;
        public int Health => _health;
        public int TurnThreshold => _turnThreshold;

        public EnemyCard(CardType type) : base(CardType.Enemy)
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

    [Serializable]
    public class EnemyPack
    {
        [SerializeField] private EnemyCard _enemyCard;
        [SerializeField] private int _count;

        public EnemyCard EnemyCard => _enemyCard;
        public int Count => _count;

        public EnemyPack(EnemyCard card, int count)
        {
            _enemyCard = card;
            _count = count;
        }

        public EnemyPack(EnemyPack pack)
        {
            _enemyCard = pack._enemyCard;
            _count = pack._count;
        }

        public void ModifyCount(int value)
        {
            _count += value;
        }

        public override string ToString()
        {
            return $"EnemyCard: {_enemyCard.Id} has {_count} amount";
        }
    }
}