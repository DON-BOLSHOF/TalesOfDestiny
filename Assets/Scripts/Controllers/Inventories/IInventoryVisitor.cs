namespace Controllers.Inventories
{
    public interface IInventoryVisitor
    {
        public void Visit(Inventory inventory);
    }
}