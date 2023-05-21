using System;
using Definitions.Inventory;
using Model.Properties;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.Disposables;

namespace Widgets.PanelWidgets.InventoryWidgets
{
    public class InventoryItemWidget : DraggableItem, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;

        [SerializeField] private InventoryItemDescriptionWidget _descriptionPanel;

        public InventoryItem Item { get; private set; }

        public ObservableProperty<InstanceStage> Stage { get; } = new ObservableProperty<InstanceStage>();
        private DescriptionStage _descriptionStage;

        private DisposeHolder _trash = new DisposeHolder();
        
        public ReactiveEvent<InventoryItemWidget> OnWidgetClicked = new ReactiveEvent<InventoryItemWidget>();
        public ReactiveEvent<InventoryItemWidget> OnItemUsed = new ReactiveEvent<InventoryItemWidget>();
        public ReactiveEvent<InventoryItem> OnItemDeleted = new ReactiveEvent<InventoryItem>();//Понадобится disable - сделаешь

        protected override void Awake()
        {
            base.Awake();
            SubscribeItem();
        }

        private void SubscribeItem()
        {
            _trash.Retain(_descriptionPanel.OnUsed.Subscribe(UseItem));
            _trash.Retain(_descriptionPanel.OnThrownOut.Subscribe(Disable));
            _trash.Retain(_descriptionPanel.SubscribeOnDisappeared(() => _descriptionStage = DescriptionStage.Close));
            _trash.Retain(Stage.SubscribeAndInvoke(delegate(InstanceStage value) { ActivateBlockRaycast(value == InstanceStage.Enabled); }));
        }

        public void OnWidgetClick()
        {
            if (Stage.Value != InstanceStage.Enabled) return;

            _descriptionPanel.Show();
            _descriptionStage = DescriptionStage.Open;

            OnWidgetClicked?.Execute(this);
        }

        public void SetData(InventoryItem item)
        {
            Item = item;
            _icon.sprite = item.Icon;
            _descriptionPanel.SetData(item);
            Stage.Value = InstanceStage.Enabled;
        }

        private void UseItem()
        {
            OnItemUsed?.Execute(this);
        }

        public void Disable()
        {
            if (Stage.Value == InstanceStage.Disabled) return;
            
            Stage.Value = InstanceStage.Disabled;
            Item = null;
            _icon.sprite = UnityUtils.LoadEmptySprite();
            ForceDescriptionExit();
        }

        public void DeleteData()
        {
            OnItemDeleted?.Execute(Item);
            Disable();//Ну тут они идентичны
        }

        public void ForceDescriptionExit()
        {
            if (_descriptionStage == DescriptionStage.Open)
                _descriptionPanel.ForceExit();
        }

        public override void OnBeginDrag(PointerEventData eventData)//Выделить бы этот блок в отдельный код...
        {
            ForceDescriptionExit();
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)//Слишком много но для 3 строк
        {
            base.OnEndDrag(eventData);
            ActivateBlockRaycast(Stage?.Value == InstanceStage.Enabled);//В силу наследия приходится подправлять базовый функционал

            CheckOuterInventoryPosition(eventData);
        }

        private void CheckOuterInventoryPosition(PointerEventData eventData)//Наиболее багомалочисленный способ, в остальных коллизии с Graphic Raycaster-aми
        {
            if (eventData.pointerEnter == null ||
                !eventData.pointerEnter.TryGetComponent<InventorySlotWidget>(out var t)) 
                DeleteData();//Самому не эмпанирует, но самый оптимальный вариант
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_descriptionStage != DescriptionStage.Close && Stage.Value == InstanceStage.Enabled)
                _descriptionPanel.StopTimer();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_descriptionStage != DescriptionStage.Close && Stage.Value == InstanceStage.Enabled)
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