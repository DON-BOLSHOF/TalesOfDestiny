using System;
using Cards.SituationCards.Event;
using UnityEngine;
using EventType = Cards.SituationCards.Event.EventType;

namespace Model.Data
{
    [Serializable]
    public class PlayerData : ICustomButtonVisitor
    {
        [SerializeField] private HeroPropertyData _propertyData;
        [SerializeField] private HeroCompanionsData _companionsData;
        [SerializeField] private HeroInventoryData _inventoryData;

        public HeroPropertyData PropertyData => _propertyData;
        public HeroCompanionsData CompanionsData => _companionsData;
        public HeroInventoryData InventoryData => _inventoryData;

        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.PropertyVisitor) != EventType.PropertyVisitor) return;
            foreach (var propertyEvent in interaction.PropertyEvents)
            {
                propertyEvent.Accept(PropertyData);
            }
        }
    }
}