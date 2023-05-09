using System;
using Cards.SituationCards.Event.ArmyEvents;
using Cards.SituationCards.Event.PropertyEvents;
using Model.Data.StorageData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cards.SituationCards.Event
{
    [Serializable]
    public class ButtonInteraction : IDataInteraction, IControllerInteraction
    {
        [SerializeField] private DataInteractionType _dataType;
        public DataInteractionType DataType => _dataType;

        [ShowIf("@(this._dataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor"), SerializeField]
        private PropertyEvent[] _propertyEvents;
        public PropertyEvent[] PropertyEvents => _propertyEvents;


        [ShowIf("@(this._dataType & DataInteractionType.ArmyVisitor) == DataInteractionType.ArmyVisitor"), SerializeField]
        private ArmyEvent[] _armyEvents;

        public ArmyEvent[] ArmyEvents => _armyEvents;

        public void Accept(IDataInteractionVisitor dataInteractionVisitor)
        {
            dataInteractionVisitor.Visit(this);
        }

        [SerializeField] private ControllerInteractionType _controllerType;
        public ControllerInteractionType ControllerType => _controllerType;

        [ShowIf("@(this._controllerType & ControllerInteractionType.Continue) == ControllerInteractionType.Continue")] [SerializeField]
        private Situation _futureSituation;

        public Situation FutureSituation => _futureSituation;

        public void Accept(IControllerInteractionVisitor controllerInteractionVisitor)
        {
            controllerInteractionVisitor.Visit(this);
        }
    }
}