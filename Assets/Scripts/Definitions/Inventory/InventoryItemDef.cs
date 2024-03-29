﻿using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Inventory
{
    [CreateAssetMenu(menuName = "Defs/InventoryItem", fileName = "InventoryItem")]
    public class InventoryItemDef : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _description;
        [SerializeField] private List<BuffDef> _buffs;
        [SerializeField] private Sprite _icon;

        public InventoryItem GetItem()
        {
            return new InventoryItem(_id, _description, _buffs, _icon);
        }
    }
}