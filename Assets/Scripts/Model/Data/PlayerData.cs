using System;
using Model.Data.StorageData;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData : IDataInteractionVisitor
    {
        [SerializeField] private HeroPropertyEventData propertyEventData;
        [SerializeField] private HeroCompanionsData _companionsData;
        [SerializeField] private HeroInventoryData _inventoryData;

        public HeroPropertyEventData PropertyEventData => propertyEventData;
        public HeroCompanionsData CompanionsData => _companionsData;
        public HeroInventoryData InventoryData => _inventoryData;

        public void Visit(IDataInteraction interaction)
        {
            if ((interaction.DataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor)
                foreach (var propertyEvent in interaction.PropertyEvents)
                {
                    propertyEvent.Accept(PropertyEventData);
                }

            if ((interaction.DataType & DataInteractionType.InventoryVisitor) ==
                DataInteractionType.InventoryVisitor)
                foreach (var inventoryEvent in interaction.InventoryEvents)
                {
                    inventoryEvent.Accept(InventoryData);
                }
        }
    }
}