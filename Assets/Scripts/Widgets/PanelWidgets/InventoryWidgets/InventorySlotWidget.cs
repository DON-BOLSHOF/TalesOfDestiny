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

        public ReactiveEvent<InventorySlotWidget> OnItemUsed = new ReactiveEvent<InventorySlotWidget>();
        public ReactiveEvent<InventorySlotWidget> OnWidgetClicked = new ReactiveEvent<InventorySlotWidget>();
        public ReactiveEvent<InventoryItem> OnThrownOut = new ReactiveEvent<InventoryItem>();
        public event Action<InventoryItem, InventorySlotWidget> OnReloaded;//Не заменен на react тк там нельзя более 1 параметра в дженерик добавлять...(

        private readonly DisposeHolder _trash = new DisposeHolder();

        private void Awake()
        {
            if (_itemWidget == null) _itemWidget = GetComponentInChildren<InventoryItemWidget>();

            SubscribeItems();
        }

        private void SubscribeItems()
        {
            _trash.Retain(_itemWidget.OnWidgetClicked.Subscribe(OnWidgetClick));
            _trash.Retain(_itemWidget.OnItemUsed.Subscribe(OnItemUse));
            _trash.Retain(_itemWidget.OnThrownOut.Subscribe(OnThrowOut));
            _trash.Retain(GetComponentInParent<InventoryPanel>().SubscribeOnChange(OnParentExit));
        }

        private void OnWidgetClick(InventoryItemWidget value)
        {
            OnWidgetClicked?.Execute(this);
        }

        private void OnItemUse(InventoryItemWidget value)
        {
            OnItemUsed?.Execute(this);
        }

        private void OnThrowOut(InventoryItem item)
        {
            OnThrownOut?.Execute(item);
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
            if (_itemWidget.Item != null) return; //Не закидываем если заполнен

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
            base.SetData(item);
            _itemWidget.SetData(item);
        }

        public override void Disable()
        {
            base.Disable();
            _itemWidget.Disable();
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