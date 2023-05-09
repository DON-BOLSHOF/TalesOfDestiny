using System;
using System.Collections;
using System.Collections.Generic;
using CodeAnimation;
using Definitions.Inventory;
using UI;
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

        [SerializeField] private PopUpHint _blockHint;
        
        private float _baseApproximation;
        private WidgetCollection<InventoryItemWidget, InventoryItem> _inventoryPanelItems;

        private Camera _mainCamera;

        private Animator _animator;

        private CameraAnimation _cameraAnimations;
        private Coroutine _routine;

        private DisposeHolder _trash = new DisposeHolder();
        
        public event Action<InventoryItem> OnDisableItem;
        public event Action<InventoryItem> OnUseItem;
        
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
                itemWidget.OnDisabled += DisposeWidget;
                itemWidget.OnUsed += UseItem;
                _trash.Retain(new ActionDisposable(() => itemWidget.OnDisabled -= DisposeWidget));
                _trash.Retain(new ActionDisposable(() => itemWidget.OnUsed -= UseItem));
            }
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

        private void UseItem(InventoryItemWidget widget)
        {
            OnUseItem?.Invoke(widget.GetData());

            var widgetIndex = _inventoryPanelItems.FindIndex(widget);
            _inventoryPanelItems.DisableAtIndex(widgetIndex);
        }

        private void DisposeWidget(InventoryItemWidget widget)
        {
            OnDisableItem?.Invoke(widget.GetData());
        }

        public void ShowBlockHint()
        {
            _blockHint.Show();
        }

        public void ForceBlockHintExit(GameState state)
        {
            if(state == GameState.None && _blockHint.IsTimerRunning) _blockHint.ForceExit();
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(coroutine);
        }
    }
}