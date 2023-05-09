using System;
using System.Collections.ObjectModel;
using Controllers;
using Definitions.Inventory;
using UnityEngine;
using Zenject;

namespace Model.Data
{
    [Serializable]
    public class HeroInventoryData : IInitializable
    {
        [SerializeField] private InventoryItem[] _inventoryItems;//Observable не сериализуем в инспекторе

        private ObservableCollection<InventoryItem> _observableInventoryItems;

        public ObservableCollection<InventoryItem>  InventoryItems => _observableInventoryItems;
        
        public void Initialize()
        {
            _observableInventoryItems = new ObservableCollection<InventoryItem>(_inventoryItems);
        }

        public void Visit(Inventory inventory, InventoryItem item)
        {
            _observableInventoryItems.Remove(item);
        }
    }
}