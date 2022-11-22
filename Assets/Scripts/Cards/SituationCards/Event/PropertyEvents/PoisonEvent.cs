using Definitions.EventDefs;
using Model.Data;
using UnityEngine;

namespace Cards.SituationCards.Event.PropertyEvents
{
    [CreateAssetMenu(menuName = "Event/PoisonEvent", fileName = "PoisonEvent")]
    public class PoisonEvent : PropertyEventDef
    {
        [SerializeField] private float _poisonModifier;

        public float PoisonModifier => _poisonModifier;
        
        public override void Accept(IPropertyVisitor visitor)
        {
            visitor.VisitPoisonEvent(this);
        }
    }
}