using UnityEngine;

namespace Cards.SituationCards
{
    [CreateAssetMenu(menuName = "Cards/SituationCard", fileName = "SituationCard")]
    public class SituationCard : LevelCard
    {
        [SerializeField] private Situation _situation;

        public Situation Situation => _situation;
        
        public SituationCard() : base(CardType.Situation)
        {
        }
    }
}