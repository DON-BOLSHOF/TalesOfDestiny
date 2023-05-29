using System;
using Model.Data.StorageData;
using Model.Tributes;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData : IDataInteractionVisitor, ITributeVisitor
    {
        [SerializeField] private HeroPropertyData _propertyData;
        [SerializeField] private HeroCompanionsData _companionsData;
        [SerializeField] private HeroInventoryData _inventoryData;

        public HeroPropertyData PropertyData => _propertyData;
        public HeroCompanionsData CompanionsData => _companionsData;
        public HeroInventoryData InventoryData => _inventoryData;

        public void Visit(IDataInteraction interaction)
        {
            if ((interaction.DataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor)
                foreach (var propertyEvent in interaction.PropertyEvents)
                {
                    propertyEvent.Accept(PropertyData);
                }

            if ((interaction.DataType & DataInteractionType.InventoryVisitor) ==
                DataInteractionType.InventoryVisitor)
                foreach (var inventoryEvent in interaction.InventoryEvents)
                {
                    inventoryEvent.Accept(InventoryData);
                }
        }

        public void Visit(ITribute tribute)
        {
            if ((tribute.Type & TributeType.PropertyTribute) == TributeType.PropertyTribute)
                PropertyData.Visit(tribute);
            
            if ((tribute.Type & TributeType.InventoryItemTribute) == TributeType.InventoryItemTribute)
                InventoryData.Visit(tribute);
        }
    }
}