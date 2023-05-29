using Definitions.Creatures.Enemies;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/BattleCard", fileName = "BattleCard")]
    public class BattleCard : EventCard
    {
        [SerializeField] private EnemyPack[] _enemyPacks;
        [SerializeField] private EnemiesType _enemiesType;
        
        public EnemyPack[] EnemyPacks => _enemyPacks;
        public EnemiesType EnemiesType => _enemiesType;

        public BattleCard() : base(LevelCardType.Battle)
        {
        }
    }
}