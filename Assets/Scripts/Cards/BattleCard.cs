using Definitions.Enemies;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/BattleCard", fileName = "BattleCard")]
    public class BattleCard : EventCard
    {
        [SerializeField] private EnemyPack[] _enemyPacks;

        public EnemyPack[] EnemyPacks => _enemyPacks;

        public BattleCard() : base(LevelCardType.Battle)
        {
        }
    }
}