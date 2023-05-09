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

        private DescriptionStage _isDescriptionStage;

        private DisposeHolder _trash = new DisposeHolder();
        public override InstanceStage InstanceStage { get; set; }

        public event Action<InventoryItemWidget> OnUsed;
        
        private void Awake()
        {
            _trash.Retain(GetComponentInParent<InventoryPanel>().SubscribeOnChange(OnParentExit));
            _trash.Retain(_descriptionPanel.SubscribeOnThrown(Disable));
            _trash.Retain(_descriptionPanel.SubscribeOnDisappeared(() => SetDescriptionStage(DescriptionStage.Close)));
            _trash.Retain(_descriptionPanel.SubscribeOnUsed(UseItem));
        }

        private void UseItem()
        {
            OnUsed?.Invoke(this);
        }

        public override void Disable()
        {
            if (InstanceStage == InstanceStage.Disabled) return;
            
            InstanceStage = InstanceStage.Disabled;
            _icon.sprite = UnityUtils.LoadEmptySprite();
            _descriptionPanel.ForceExit();
            OnDisabled?.Invoke(this);
        }

        public override void SetData(InventoryItem item)
        {
            _item = item;
            _icon.sprite = item.Icon;
            _descriptionPanel.SetData(item);
            InstanceStage = InstanceStage.Enabled;
        }

        public override InventoryItem GetData()
        {
            return _item;
        }

        public void ShowDescription()
        {
            if (InstanceStage != InstanceStage.Enabled) return;
            
            _descriptionPanel.Show();
            _isDescriptionStage = DescriptionStage.Open;
        }

        private void SetDescriptionStage(DescriptionStage stage)
        {
            _isDescriptionStage = stage;
        }

        private void OnParentExit(bool value)
        {
            if(!value)
            { 
                _descriptionPanel.ForceExit();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isDescriptionStage != DescriptionStage.Close && InstanceStage == InstanceStage.Enabled)
                _descriptionPanel.StopTimer();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isDescriptionStage != DescriptionStage.Close && InstanceStage == InstanceStage.Enabled)
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