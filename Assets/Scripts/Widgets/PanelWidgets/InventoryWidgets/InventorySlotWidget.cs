using System;
using Definitions.Inventory;
using Panels;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Utils.Disposables;

namespace Widgets.PanelWidgets.InventoryWidgets
{
    public class
        InventorySlotWidget : WidgetInstance<InventorySlotWidget, InventoryItem>, IDropHandler //Чистая прослойка
    {
        [SerializeField] private InventoryItemWidget _itemWidget;

        public event Action<InventorySlotWidget> OnItemUsed;
        public event Action<InventorySlotWidget> OnWidgetClicked;
        public event Action<InventoryItem, InventorySlotWidget> OnReloaded;

        private DisposeHolder _trash = new DisposeHolder();

        private void Awake()
        {
            if (_itemWidget == null) _itemWidget = GetComponentInChildren<InventoryItemWidget>();

            SubscribeItems();
        }

        private void SubscribeItems()
        {
            _itemWidget.OnWidgetClicked += OnWidgetClick;
            _itemWidget.OnItemUsed += OnItemUse;
            _itemWidget.OnItemDeleted += OnDeleteData;
            _trash.Retain(new ActionDisposable(() => _itemWidget.OnWidgetClicked -= OnWidgetClick));
            _trash.Retain(new ActionDisposable(() => _itemWidget.OnItemUsed -= OnItemUse));
            _trash.Retain(new ActionDisposable(() => _itemWidget.OnItemDeleted -= OnDeleteData));
            _trash.Retain(GetComponentInParent<InventoryPanel>().SubscribeOnChange(OnParentExit));
        }

        private void OnWidgetClick(InventoryItemWidget value)
        {
            OnWidgetClicked?.Invoke(this);
        }

        private void OnItemUse(InventoryItemWidget value)
        {
            OnItemUsed?.Invoke(this);
        }

        private void OnDeleteData(InventoryItem value)
        {
            OnDeletedData?.Invoke(value);
        }

        private void OnParentExit(bool value)
        {
            if (!value)
            {
                HideClickConsequences();
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(_itemWidget.Item != null) return;//Не закидываем если заполнен

            if (eventData.pointerDrag.TryGetComponent(out InventoryItemWidget widget))
            {
                ReloadSlot(widget); // Недавно закон Деметры узнал ;)))
            }
        }

        private void ReloadSlot(InventoryItemWidget widget)
        {
            var item = widget.Item;
            OnReloaded?.Invoke(item, this);
        }

        public void HideClickConsequences()
        {
            _itemWidget.ForceDescriptionExit();
        }

        public override void SetData(InventoryItem item)
        {
            _itemWidget.SetData(item);
        }

        public override void Disable()
        {
            _itemWidget.Disable();
        }

        public override void DeleteData()
        {
            _itemWidget.DeleteData();
        }

        public override InventoryItem GetData()
        {
            return _itemWidget.Item;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}