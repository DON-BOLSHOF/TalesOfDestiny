using Controllers;

namespace Utils.Interfaces
{
    public interface IInventoryVisitor
    {
        public void Visit(Inventory inventory);
    }
}