using System;
using Cards.SituationCards.Event.ArmyEvents;
using Cards.SituationCards.Event.PropertyEvents;
using Model.Data;
using Model.Data.ControllersData;
using Model.Data.StorageData;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Interfaces;

namespace Cards.SituationCards.Event
{
    [Serializable]
    public class ButtonInteraction : IDataInteraction, IControllerInteraction
    {
        [SerializeField] private DataInteractionType _dataType;

        [ShowIf("@(this._dataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor"), SerializeField]
        private PropertyEvent[] _propertyEvents;

        [ShowIf("@(this._dataType & DataInteractionType.ArmyVisitor) == DataInteractionType.ArmyVisitor"), SerializeField]
        private ArmyEvent[] _armyEvents;

        [SerializeField] private ControllerInteractionType _controllerType;

        [ShowIf("@(this._controllerType & ControllerInteractionType.Continue) == ControllerInteractionType.Continue")] [SerializeField]
        private Situation _futureSituation;

        public DataInteractionType DataType => _dataType;
        public PropertyEvent[] PropertyEvents => _propertyEvents;
        public ArmyEvent[] ArmyEvents => _armyEvents;
        public ControllerInteractionType ControllerType => _controllerType;

        public Situation FutureSituation => _futureSituation;

        public void Accept(IDataInteractionVisitor dataInteractionVisitor)
        {
            dataInteractionVisitor.Visit(this);
        }

        public void Accept(IControllerInteractionVisitor controllerInteractionVisitor)
        {
            controllerInteractionVisitor.Visit(this);
        }
    }
}