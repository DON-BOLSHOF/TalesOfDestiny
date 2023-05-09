using System;
using Model.Data.StorageData;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData : IDataInteractionVisitor
    {
        [SerializeField] private HeroPropertyData _propertyData;
        [SerializeField] private HeroCompanionsData _companionsData;
        [SerializeField] private HeroInventoryData _inventoryData;

        public HeroPropertyData PropertyData => _propertyData;
        public HeroCompanionsData CompanionsData => _companionsData;
        public HeroInventoryData InventoryData => _inventoryData;

        public void Visit(IDataInteraction interaction)
        {
            if ((interaction.DataType & DataInteractionType.PropertyVisitor) != DataInteractionType.PropertyVisitor) return;
            foreach (var propertyEvent in interaction.PropertyEvents)
            {
                propertyEvent.Accept(PropertyData);
            }
        }
    }
}