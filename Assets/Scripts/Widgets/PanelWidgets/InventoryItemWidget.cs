using System;
using Definitions.Inventory;
using Panels;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.Disposables;

namespace Widgets.PanelWidgets
{
    public class InventoryItemWidget : WidgetInstance<InventoryItemWidget, InventoryItem>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;

        [SerializeField] private InventoryItemDescriptionWidget _descriptionPanel;

        private InventoryItem _item;

        public DescriptionStage DescriptionStage { get; private set; }
        public override InstanceStage InstanceStage { get; set; }

        private DisposeHolder _trash = new DisposeHolder();
        public event Action<InventoryItemWidget> OnWidgetClicked; 
        public event Action<InventoryItemWidget> OnItemUsed;

        private void Awake()
        {
            _trash.Retain(_descriptionPanel.SubscribeOnUsed(UseItem));
            _trash.Retain(_descriptionPanel.SubscribeOnThrown(Disable));
            _trash.Retain(_descriptionPanel.SubscribeOnDisappeared(() => SetDescriptionStage(DescriptionStage.Close)));
            _trash.Retain(GetComponentInParent<InventoryPanel>().SubscribeOnChange(OnParentExit));
        }

        public void OnWidgetClick()
        {
            if (InstanceStage != InstanceStage.Enabled) return;
            
            _descriptionPanel.Show();
            DescriptionStage = DescriptionStage.Open;
            
            OnWidgetClicked?.Invoke(this);
        }

        public override void SetData(InventoryItem item)
        {
            _item = item;
            _icon.sprite = item.Icon;
            _descriptionPanel.SetData(item);
            InstanceStage = InstanceStage.Enabled;
        }

        private void UseItem()
        {
            OnItemUsed?.Invoke(this);
        }

        public override void Disable()
        {
            if (InstanceStage == InstanceStage.Disabled) return;
            
            InstanceStage = InstanceStage.Disabled;
            _icon.sprite = UnityUtils.LoadEmptySprite();
            ForceDescriptionExit();
            OnDisabled?.Invoke(this);
        }

        private void SetDescriptionStage(DescriptionStage stage)
        {
            DescriptionStage = stage;
        }

        private void OnParentExit(bool value)
        {
            if(!value)
            { 
                ForceDescriptionExit();
            }
        }

        public void ForceDescriptionExit()
        {
            _descriptionPanel.ForceExit();
        }

        public override InventoryItem GetData()
        {
            return _item;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (DescriptionStage != DescriptionStage.Close && InstanceStage == InstanceStage.Enabled)
                _descriptionPanel.StopTimer();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (DescriptionStage != DescriptionStage.Close && InstanceStage == InstanceStage.Enabled)
                _descriptionPanel.ReloadTimer();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }

    public enum DescriptionStage
    {
        Close,
        Open,
    }
}