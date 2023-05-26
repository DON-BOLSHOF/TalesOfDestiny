using Definitions.EventDefs;
using UnityEngine;
using Utils.Interfaces;
using Utils.Interfaces.Visitors;

namespace Cards.SituationCards.Event.PropertyEvents
{
    [CreateAssetMenu(menuName = "Event/PoisonEvent", fileName = "PoisonEvent")]
    public class PoisonEvent : PropertyEventDef
    {
        [SerializeField] private int _poisonModifier;

        public int PoisonModifier => _poisonModifier;
        
        public override void Accept(IPropertyEventVisitor eventVisitor)
        {
            eventVisitor.VisitPoisonEvent(this);
        }
    }
}