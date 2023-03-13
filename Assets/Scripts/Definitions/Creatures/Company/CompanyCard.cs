using Cards;
using UnityEngine;

namespace Definitions.Creatures.Company
{
    
    [CreateAssetMenu(menuName = "Defs/Creature/Companion", fileName = "Companion")]
    public class CompanyCard : CreatureCard
    {
        public CompanyCard(CardType type) : base(CardType.Company)
        {
        }
    }
}