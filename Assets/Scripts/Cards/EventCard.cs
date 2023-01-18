using Cards.SituationCards;
using UnityEngine;

namespace Cards
{
    public class EventCard : LevelCard
    {
        [SerializeField] private Situation _situation;

        public Situation Situation => _situation;
        
        public EventCard(LevelCardType type) : base(CardType.Event, type)
        {
        }
    }
}