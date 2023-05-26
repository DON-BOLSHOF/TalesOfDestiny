using Definitions.EventDefs;
using UnityEngine;
using Utils.Interfaces;
using Utils.Interfaces.Visitors;

namespace Cards.SituationCards.Event.PropertyEvents
{
    [CreateAssetMenu(menuName = "Event/BurningCampEvent", fileName = "BurningCampEvent")]
    public class BurningCampEvent : PropertyEventDef
    {
        [SerializeField, Range(0,1)] private float _damagePerCent;

        public float DamagePerCent => _damagePerCent;
        
        public override void Accept(IPropertyEventVisitor eventVisitor)
        {
            eventVisitor.VisitBurningCampEvent(this);
        }
    }
}