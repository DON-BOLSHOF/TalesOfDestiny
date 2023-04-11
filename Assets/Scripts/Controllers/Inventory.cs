using Panels;
using UnityEngine;

namespace Controllers
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private InventoryPanel _inventoryPanel;

        public void Show()
        {
            _inventoryPanel.gameObject.SetActive(true);
            _inventoryPanel.Show();
        }
    }
}