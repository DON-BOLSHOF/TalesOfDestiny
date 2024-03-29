﻿using System;
using Cards.SituationCards.Event.ArmyEvents;
using Cards.SituationCards.Event.InventoryEvents;
using Cards.SituationCards.Event.PropertyEvents;

namespace Model.Data.StorageData
{
    public interface IDataInteraction
    {
        DataInteractionType DataType { get; }
        PropertyEvent[] PropertyEvents { get; }
        ArmyEvent[] ArmyEvents { get; }
        InventoryEvent[] InventoryEvents{ get;}

        void Accept(IDataInteractionVisitor visitor);
    }
    
    [Flags]
    public enum DataInteractionType
    {
        None = 0,
        ArmyVisitor = 1,
        PropertyVisitor = 2,
        InventoryVisitor = 4,
    }
}