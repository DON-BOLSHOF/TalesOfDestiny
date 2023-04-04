using Cards;
using UnityEngine;

namespace Definitions.Creatures.Company
{
    
    [CreateAssetMenu(menuName = "Defs/Creature/Companion", fileName = "Companion")]
    public class CompanionCard : CreatureCard
    {
        public CompanionCard(CardType type) : base(CardType.Companion)
        {
        }
    }
}