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
        [SerializeField] private PropertyEvent[] _propertyEvents;
        [SerializeField] private ArmyEvent[] _armyEvents;
        [SerializeField] private Situation _futureSituation;

        public EventType Type => _type;
        public PropertyEvent[] PropertyEvents => _propertyEvents;
        public ArmyEvent[] ArmyEvents => _armyEvents;
        public Situation FutureSituation => _futureSituation;

        public ButtonVisitor SetButtonVisitor(ICustomButtonVisitor customButtonVisitor)
        {
            return new ButtonVisitor(customButtonVisitor, this);
        }
        
        public class ButtonVisitor
        {
            private ButtonInteraction _interaction;
            private ICustomButtonVisitor _customButtonVisitor;

            public ButtonVisitor(ICustomButtonVisitor customButtonVisitor, ButtonInteraction interaction)
            {
                _customButtonVisitor = customButtonVisitor;
                _interaction = interaction;
            }

            public void OnClick()
            {
                _customButtonVisitor.Visit(_interaction);
            }
        }
    }

    [Flags]
    public enum EventType : short
    {
        None = 0,
        ArmyVisitor = 1,
        PropertyVisitor = 2,
        EquipVisitor = 4,
        Continue = 8,
        ClosePanel = 16,
        EndJourney = 32, 
        Battle = 64
    }
}