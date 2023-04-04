using Definitions.EventDefs;
using Model.Data;
using UnityEngine;
using Utils.Interfaces;

namespace Cards.SituationCards.Event.PropertyEvents
{
    [CreateAssetMenu(menuName = "Event/PoisonEvent", fileName = "PoisonEvent")]
    public class PoisonEvent : PropertyEventDef
    {
        [SerializeField] private int _poisonModifier;

        public int PoisonModifier => _poisonModifier;
        
        public override void Accept(IPropertyVisitor visitor)
        {
            visitor.VisitPoisonEvent(this);
        }
    }
}