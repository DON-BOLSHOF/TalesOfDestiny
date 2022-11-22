using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/EnemyCard", fileName = "EnemyCard")]
    public class EnemyCard: LevelCard
    {
        public EnemyCard(CardType type) : base(type)
        {
        }
    }
}