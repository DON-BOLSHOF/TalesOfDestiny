using System;
using UnityEngine;
using View.EnemyView;

namespace Definitions.Enemies
{
    [CreateAssetMenu(menuName = "Defs/Enemy", fileName = "Enemy")]
    public class Enemy : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private СharacteristicType _сharacteristicType;
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private int _attack;
        [SerializeField] private int _health;
        [SerializeField] private int _armor;

        public string Id => _id;
        public СharacteristicType СharacteristicType => _сharacteristicType;
        public EnemyView EnemyView => _enemyView;
        public int Attack => _attack;
        public int Health => _health;
        public int Armor => _armor;
    }

    [Serializable]
    public enum СharacteristicType
    {
        Strength,
        Agility,
        Intelligent
    }

    [Serializable]
    public class EnemyPack
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private int _count;

        public Enemy Enemy => _enemy;
        public int Count => _count;
    }
}