using System;
using System.Collections.ObjectModel;
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
        

        public void AddItem(InventoryItem item)
        {
            _observableInventoryItems.Add(item);
        }

        public void Initialize()
        {
            _observableInventoryItems = new ObservableCollection<InventoryItem>(_inventoryItems);
        }
    }
}