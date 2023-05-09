using System;
using System.Collections;
using System.Collections.Generic;
using CodeAnimation;
using Definitions.Inventory;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Widgets.PanelWidgets;

namespace Panels
{
    [RequireComponent(typeof(Animator))]
    public class InventoryPanel : AbstractPanelUtil
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private float _approximation;
        
        private WidgetCollection<InventoryItemWidget, InventoryItem> _inventoryPanelItems;
        private InventoryItemWidget _currentDescriptionOpenWidget;

        private float _baseApproximation;
        private Camera _mainCamera;
        private CameraAnimation _cameraAnimations;

        private Animator _animator;

        private Coroutine _routine;

        private DisposeHolder _trash = new DisposeHolder();

        public event Action<InventoryItem> OnUseItem;
        public event Action<InventoryItem> OnDisableItem;

        private static readonly int Showing = Animator.StringToHash("Showing");
        private static readonly int Exiting = Animator.StringToHash("Exiting");

        private void Awake()
        {
            Initialize();
            SubscribeItems();
        }

        private void Initialize()
        {
            _inventoryPanelItems = new WidgetCollection<InventoryItemWidget, InventoryItem>(_itemsContainer);

            _mainCamera = Camera.main;
            if (_mainCamera == null) throw new ArgumentException("Camera not found");
            _baseApproximation = _mainCamera.fieldOfView;

            _animator = GetComponent<Animator>();
            _cameraAnimations = new CameraAnimation(_baseApproximation, _approximation);
        }

        private void SubscribeItems()
        {
            foreach (var itemWidget in _inventoryPanelItems)
            {
                itemWidget.OnItemUsed += UseItem;
                itemWidget.OnWidgetClicked += HideOtherWidgetDescription;
                itemWidget.OnDisabled += DisposeWidget;
                _trash.Retain(new ActionDisposable(() => itemWidget.OnItemUsed -= UseItem));
                _trash.Retain(new ActionDisposable(() => itemWidget.OnWidgetClicked -= HideOtherWidgetDescription));
                _trash.Retain(new ActionDisposable(() => itemWidget.OnDisabled -= DisposeWidget));
            }
        }

        private void UseItem(InventoryItemWidget widget)
        {
            OnUseItem?.Invoke(widget.GetData());

            var widgetIndex = _inventoryPanelItems.FindIndex(widget);
            _inventoryPanelItems.DisableAtIndex(widgetIndex);
        }

        private void HideOtherWidgetDescription(InventoryItemWidget widget)
        {
            if (_currentDescriptionOpenWidget != null && !_currentDescriptionOpenWidget.Equals(widget) &&
                _currentDescriptionOpenWidget.DescriptionStage == DescriptionStage.Open)
                _currentDescriptionOpenWidget.ForceDescriptionExit();

            _currentDescriptionOpenWidget = widget;
        }

        private void DisposeWidget(InventoryItemWidget widget)
        {
            OnDisableItem?.Invoke(widget.GetData());
        }

        public override void Show()
        {
            _animator.SetTrigger(Showing);
            OnChangeState?.Invoke(true);

            StartRoutine(_cameraAnimations.Approximating(_mainCamera, ApproximationType.Approximate));
        }

        public override void Exit()
        {
            _animator.SetTrigger(Exiting);
            OnChangeState?.Invoke(false);

            StartRoutine(_cameraAnimations.Approximating(_mainCamera, ApproximationType.Estrange));
        }

        public void InitializeItems(List<InventoryItem> inventoryItems)
        {
            _inventoryPanelItems.ReloadData(inventoryItems);
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}