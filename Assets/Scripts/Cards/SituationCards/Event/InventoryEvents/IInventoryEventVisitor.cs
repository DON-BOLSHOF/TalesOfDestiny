namespace Cards.SituationCards.Event.InventoryEvents
{
    public interface IInventoryEventVisitor
    {
        void Accept(InventoryEvent inventoryEvent);
    }
}