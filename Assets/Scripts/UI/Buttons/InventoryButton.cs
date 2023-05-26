using Controllers;
using Controllers.Inventories;
using Zenject;

namespace UI.Buttons
{
    public class InventoryButton : MagicScrollButton
    {
        [Inject] private Inventory _inventory;
         
        public override void Cast()
        {
            _inventory.Show();
        }
    }
}