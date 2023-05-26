using Cards.SituationCards.Event.PropertyEvents;
using UnityEngine;

namespace Definitions.EventDefs
{
    public abstract class PropertyEventDef : ScriptableObject
    {
        public abstract void Accept(IPropertyEventVisitor eventVisitor);
    }
}