using Definitions.Enemies;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/EnemyCard", fileName = "EnemyCard")]
    public class EnemyCard: EventCard
    {
        [SerializeField] private EnemyPack[] _enemies;
        
        public EnemyCard() : base(LevelCardType.Enemy)
        {
        }
    }
}