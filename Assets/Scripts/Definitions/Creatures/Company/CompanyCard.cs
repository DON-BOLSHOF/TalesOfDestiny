using Cards;
using UnityEngine;

namespace Definitions.Creatures.Company
{
    
    [CreateAssetMenu(menuName = "Defs/Companion", fileName = "Companion")]
    public class CompanyCard : CreatureCard
    {
        public CompanyCard(CardType type) : base(CardType.Company)
        {
        }
    }
}