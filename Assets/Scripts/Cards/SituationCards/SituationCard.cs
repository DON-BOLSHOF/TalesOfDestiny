using UnityEngine;

namespace Cards.SituationCards
{
    [CreateAssetMenu(menuName = "Cards/SituationCard", fileName = "SituationCard")]
    public class SituationCard : EventCard
    {
        public SituationCard() : base(LevelCardType.Situation)
        {
        }
    }
}