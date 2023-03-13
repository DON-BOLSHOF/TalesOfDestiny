using Cards;
using UnityEngine;

namespace Definitions.Creatures.Enemies
{
    [CreateAssetMenu(menuName = "Defs/Creature/Enemy", fileName = "Enemy")]
    public class EnemyCard : CreatureCard
    {
        [SerializeField] private int _turnThreshold;
        
        public int TurnThreshold => _turnThreshold;

        public EnemyCard(CardType type) : base(CardType.Enemy)
        {
        }
    }
}