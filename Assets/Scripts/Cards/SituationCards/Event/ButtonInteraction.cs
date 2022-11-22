using System;
using Cards.SituationCards.Event.ArmyEvents;
using Cards.SituationCards.Event.PropertyEvents;
using UnityEngine;

namespace Cards.SituationCards.Event
{
    [Serializable]
    public class ButtonInteraction
    {
        [SerializeField] private EventType _type;
        [SerializeField] private PropertyEvent[] _propertyEvent;
        [SerializeField] private ArmyEvent[] _armyEvent;
    }
    
    [Flags]
    public enum EventType: short
    {
        None = 0,
        ArmyVisitor = 1,
        PropertyVisitor = 2,
        EquipVisitor = 4,
        Continue = 8,
        Close = 16,
        Everything = 31
    }
}