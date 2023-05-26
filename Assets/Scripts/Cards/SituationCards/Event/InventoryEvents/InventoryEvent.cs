using System;
using Definitions.Inventory;
using UnityEngine;

namespace Cards.SituationCards.Event.InventoryEvents
{
    [Serializable]
    public class InventoryEvent
    {
        [SerializeField] private InventoryItemDef _itemDef;

        public InventoryItem Item => _itemDef.GetItem();

        public void Accept(IInventoryEventVisitor inventoryVisitor)
        {
            inventoryVisitor.Accept(this);
        }
    }
}