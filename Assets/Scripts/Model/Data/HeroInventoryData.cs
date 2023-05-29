using System;
using System.Collections.ObjectModel;
using System.Linq;
using Cards.SituationCards.Event.InventoryEvents;
using Controllers.Inventories;
using Definitions.Inventory;
using Model.Tributes;
using UnityEngine;
using Zenject;

namespace Model.Data
{
    [Serializable]
    public class HeroInventoryData : IInitializable, IInventoryEventVisitor
    {
        [SerializeField] private InventoryItemDef[] _inventoryItems;//Observable не сериализуем в инспекторе

        private ObservableCollection<InventoryItem> _observableInventoryItems;

        public ObservableCollection<InventoryItem>  InventoryItems => _observableInventoryItems;
        
        public void Initialize()
        {
            var instantiatedItems = InstantiateItems();

            _observableInventoryItems = new ObservableCollection<InventoryItem>(instantiatedItems);
        }

        private InventoryItem[] InstantiateItems()
        {
            var instantiatedItems = new InventoryItem[_inventoryItems.Length];
            for (var i = 0; i < _inventoryItems.Length; i++)
            {
                instantiatedItems[i] = _inventoryItems[i].GetItem();
            }

            return instantiatedItems;
        }

        public void Visit(ITribute tribute)
        {
            foreach (var inventoryItem in tribute.TributeInventoryItems)
            {
                _observableInventoryItems.Remove(_observableInventoryItems.First(item => item.Id.Equals(inventoryItem.Id)));
            }
        }

        public void Visit(Inventory inventory, InventoryItem item)
        {
            _observableInventoryItems.Remove(item);
        }

        public void Accept(InventoryEvent inventoryEvent)
        {
            _observableInventoryItems.Add(inventoryEvent.Item);
        }
    }
}