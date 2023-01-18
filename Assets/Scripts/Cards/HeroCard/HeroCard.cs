using UnityEngine;

namespace Cards.HeroCard
{
    [CreateAssetMenu(menuName = "Cards/HeroCard", fileName = "HeroCard")]
    public class HeroCard : LevelCard
    {
        public HeroCard() : base(CardType.General, LevelCardType.HeroPosition)
        {
        }
    }
}