using System;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private string _id;
        [SerializeField] private string _description;
        [SerializeField] private List<BuffDef> _buffs;

        public string Id => _id;
        public string Description => _description;
        public List<BuffDef> Buffs => _buffs;
    }
}