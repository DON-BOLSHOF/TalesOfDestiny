using System;
using Definitions.Inventory;
using Model.Data.StorageData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Model.Tributes
{
    [Serializable]
    public class Tribute : ITribute
    {
        [SerializeField] private TributeType _tributeType;

        [ShowIf("@(this._tributeType & TributeType.PropertyTribute) == TributeType.PropertyTribute"), SerializeField]
        private PropertyData _propertyData;

        [ShowIf("@(this._tributeType & TributeType.InventoryItemTribute) == TributeType.InventoryItemTribute"), SerializeField]
        private InventoryItemDef[] _inventoryItemDef;

        public TributeType Type => _tributeType;
        public PropertyData TributePropertyData => _propertyData;
        public InventoryItem[] TributeInventoryItems => Array.ConvertAll(_inventoryItemDef, input => input.GetItem());

        public void Accept(ITributeVisitor tributeVisitor)
        {
            tributeVisitor.Visit(this);
        }
    }
}