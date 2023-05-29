using System;
using Definitions.Inventory;
using Model.Data.StorageData;

namespace Model.Tributes
{
    public interface ITribute
    {
        public TributeType Type { get; }
        public PropertyData TributePropertyData { get; }
        public InventoryItem[] TributeInventoryItems { get; }

        public void Accept(ITributeVisitor tributeVisitor);
    }

    [Flags]
    public enum TributeType
    {
        None = 0,
        PropertyTribute = 1,
        InventoryItemTribute = 2
    }
}