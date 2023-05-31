using Definitions.Inventory;
using Model.Properties;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.DataGroups;
using Utils.Disposables;

namespace Widgets.PanelWidgets.InventoryWidgets
{
    public class InventoryItemWidget : DraggableItem, IPointerEnterHandler, IPointerExitHandler//Вроде код неплох, но SRP там тут нарушается - исправить по заветам дяди Боба
    {
        [SerializeField] private Image _icon;

        [SerializeField] private InventoryItemDescriptionWidget _descriptionPanel;

        public InventoryItem Item { get; private set; }

        private ObservableProperty<InstanceStage> Stage { get; } = new ObservableProperty<InstanceStage>();
        private DescriptionStage _descriptionStage;

        private DisposeHolder _trash = new DisposeHolder();
        
        public ReactiveEvent<InventoryItemWidget> OnWidgetClicked = new ReactiveEvent<InventoryItemWidget>();
        public ReactiveEvent<InventoryItemWidget> OnItemUsed = new ReactiveEvent<InventoryItemWidget>();
        public ReactiveEvent<InventoryItem> OnThrownOut = new ReactiveEvent<InventoryItem>();

        protected override void Awake()
        {
            base.Awake();
            SubscribeItem();
        }

        private void SubscribeItem()
        {
            _trash.Retain(_descriptionPanel.OnUsed.Subscribe(OnUseItem));
            _trash.Retain(_descriptionPanel.OnThrownOut.Subscribe(OnThrowOut));
            _trash.Retain(_descriptionPanel.OnDisappeared.Subscribe(() => _descriptionStage = DescriptionStage.Close));
            _trash.Retain(Stage.SubscribeAndInvoke(delegate(InstanceStage value) { ActivateBlockRaycast(value == InstanceStage.Activated); }));
        }

        private void OnUseItem()
        {
            OnItemUsed?.Execute(this);
        }

        private void OnThrowOut()
        {
            OnThrownOut?.Execute(Item);
        }

        public void SetData(InventoryItem item)
        {
            Item = item;
            _icon.sprite = item.Icon;
            _descriptionPanel.SetData(item);
            Stage.Value = InstanceStage.Activated;
        }

        public void Disable()
        {
            if (Stage.Value == InstanceStage.Disabled) return;
            
            Stage.Value = InstanceStage.Disabled;
            Item = null;
            _icon.sprite = UnityUtils.LoadEmptySprite();
            ForceDescriptionExit();
        }

        public void ForceDescriptionExit()
        {
            if (_descriptionStage == DescriptionStage.Open)
                _descriptionPanel.ForceExit();
        }

        private void OnWidgetClick()
        {
            if (Stage.Value != InstanceStage.Activated) return;

            _descriptionPanel.Show();
            _descriptionStage = DescriptionStage.Open;

            OnWidgetClicked?.Execute(this);
        }

        public override void OnBeginDrag(PointerEventData eventData)//Выделить бы этот блок в отдельный код...
        {
            ForceDescriptionExit();
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)//Слишком много но для 3 строк
        {
            base.OnEndDrag(eventData);
            ActivateBlockRaycast(Stage?.Value == InstanceStage.Activated);//В силу наследия приходится подправлять базовый функционал

            CheckOuterInventoryPosition(eventData);
        }

        private void CheckOuterInventoryPosition(PointerEventData eventData)//Наиболее багомалочисленный способ, в остальных коллизии с Graphic Raycaster-aми
        {
            if (eventData.pointerEnter == null ||
                eventData.pointerEnter.GetComponentInParent<InventorySlotWidget>() == null) 
                OnThrowOut();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_descriptionStage != DescriptionStage.Close && Stage.Value == InstanceStage.Activated)
                _descriptionPanel.StopTimer();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_descriptionStage != DescriptionStage.Close && Stage.Value == InstanceStage.Activated)
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