using System;
using Cards.SituationCards.Event.ArmyEvents;
using Cards.SituationCards.Event.InventoryEvents;
using Cards.SituationCards.Event.PropertyEvents;
using Model.Data.StorageData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Definitions.Inventory
{
    [Serializable]
    public class BuffDef : IDataInteraction
    {
        [SerializeField] private BuffState buffState;
        public BuffState BuffState => buffState;

        [SerializeField] private DataInteractionType _dataInteraction;
        public DataInteractionType DataType => _dataInteraction;
        
        [ShowIf("@(this._dataInteraction & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor"), SerializeField]
        private PropertyEvent[] _propertyEvents;
        public PropertyEvent[] PropertyEvents => _propertyEvents;


        [ShowIf("@(this._dataInteraction & DataInteractionType.ArmyVisitor) == DataInteractionType.ArmyVisitor"), SerializeField]
        private ArmyEvent[] _armyEvents;
        public ArmyEvent[] ArmyEvents => _armyEvents;
        
        [ShowIf("@(this._dataInteraction & DataInteractionType.InventoryVisitor) == DataInteractionType.InventoryVisitor"), SerializeField]
        private InventoryEvent[] _inventoryEvents;
        public InventoryEvent[] InventoryEvents => _inventoryEvents;

        public void Accept(IDataInteractionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    public enum BuffState // Используется внутри инвенторя(условные кнопки пополняющие еду), Или чисто перетаскиваемые шмотки в армию/куда-нибудь еще мб понадобится 
    {
        Active,
        Passive,//На Army накидывается или мб чет еще будет
    }
}