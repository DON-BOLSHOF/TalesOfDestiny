using System;
using Definitions.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Widgets.PanelWidgets
{
    public class InventoryItemWidget : MonoBehaviour, IWidgetInstance<InventoryItem>
    {
        [SerializeField] private Image _icon;

        [SerializeField] private InventoryItemDescriptionWidget _hint;

        private InventoryItem _item;

        public InstanceStage InstanceStage { get; private set; }

        private void Awake()
        {
            _hint.OnThrownOut += Disable;
        }

        public void SetData(InventoryItem item)
        {
            _item = item;
            _hint.SetData(item);
            InstanceStage = InstanceStage.Enabled;
        }

        public InventoryItem GetData()
        {
            return _item;
        }

        public void Disable()
        {
            InstanceStage = InstanceStage.Disabled;
            throw new System.NotImplementedException();
        }

        private void OnDestroy()
        {
            _hint.OnThrownOut -= Disable;
        }
    }
}