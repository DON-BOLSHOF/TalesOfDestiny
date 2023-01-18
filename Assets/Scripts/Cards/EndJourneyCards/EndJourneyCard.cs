using UnityEngine;

namespace Cards.EndJourneyCards
{
    [CreateAssetMenu(menuName = "Cards/EndJourneyCard", fileName = "EndJourneyCard")]
    public class EndJourneyCard : EventCard
    {
        public EndJourneyCard() : base(LevelCardType.EndJourney)
        {
        }
    }
}