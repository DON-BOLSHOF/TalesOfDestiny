using Cards.SituationCards.PhysicalCard;
using UnityEngine;

namespace Cards.SituationCards
{
    [CreateAssetMenu(menuName = "Cards/SituationCard", fileName = "SituationCard")]
    public class SituationCard : LevelCard
    {
        [SerializeField] private PhysicalCardView _cardView;
        [SerializeField] private Situation _situation;

        public PhysicalCardView CardView => _cardView;
        public Situation Situation => _situation;
        
        public SituationCard() : base(CardType.Situation)
        {
        }
    }
}