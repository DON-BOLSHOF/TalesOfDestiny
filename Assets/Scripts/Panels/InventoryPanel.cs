using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using CodeAnimation;
using Definitions.Inventory;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Widgets.PanelWidgets.InventoryWidgets;

namespace Panels
{
    [RequireComponent(typeof(Animator))]
    public class InventoryPanel : AbstractPanelUtil
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private float _approximation;

        private WidgetCollection<InventorySlotWidget, InventoryItem> _inventoryPanelSlots;
        private InventorySlotWidget _lastClickedWidget;

        private float _baseApproximation; //Крайне не очевидная логика
        private Camera _mainCamera;
        private CameraAnimation _cameraAnimations;

        private Animator _animator;

        private Coroutine _routine;

        private DisposeHolder _trash = new DisposeHolder();

        public ReactiveEvent<InventoryItem> OnUseItem = new ReactiveEvent<InventoryItem>();
        public ReactiveEvent<InventoryItem> OnThrownOut = new ReactiveEvent<InventoryItem>();

        private static readonly int Showing = Animator.StringToHash("Showing");
        private static readonly int Exiting = Animator.StringToHash("Exiting");

        private void Awake()
        {
            Initialize();
            SubscribeItems();
        }

        private void Initialize()
        {
            _inventoryPanelSlots = new WidgetCollection<InventorySlotWidget, InventoryItem>(_itemsContainer);

            _mainCamera = Camera.main;
            if (_mainCamera == null) throw new ArgumentException("Camera not found");
            _baseApproximation = _mainCamera.fieldOfView;

            _animator = GetComponent<Animator>();
            _cameraAnimations = new CameraAnimation(_baseApproximation, _approximation);
        }

        private void SubscribeItems()
        {
            foreach (var slotWidget in _inventoryPanelSlots)
            {
                slotWidget.OnReloaded += ReloadDraggableItem;
                _trash.Retain(new ActionDisposable(() => slotWidget.OnReloaded -= ReloadDraggableItem));
                _trash.Retain(slotWidget.OnItemUsed.Subscribe(OnUsedItem));
                _trash.Retain(slotWidget.OnWidgetClicked.Subscribe(HideOtherWidgetClickConsequences));
                _trash.Retain(slotWidget.OnThrownOut.Subscribe(OnThrownOutItem));
            }
        }

        private void ReloadDraggableItem(InventoryItem changingData, InventorySlotWidget slotToChange)
        {
            var index = _inventoryPanelSlots.FindIndex(changingData);
            _inventoryPanelSlots.DisableAtIndex(index);

            slotToChange.SetData(changingData);
        }

        private void OnUsedItem(InventorySlotWidget widget)
        {
            OnUseItem?.Execute(widget.GetData());
        }

        private void HideOtherWidgetClickConsequences(InventorySlotWidget widget)
        {
            if (_lastClickedWidget != null && !_lastClickedWidget.Equals(widget))
                _lastClickedWidget.HideClickConsequences();

            _lastClickedWidget = widget;
        }

        private void OnThrownOutItem(InventoryItem widget)
        {
            OnThrownOut?.Execute(widget);
        }

        public void InitializeSlots(List<InventoryItem> inventoryItems)
        {
            _inventoryPanelSlots.ReloadData(inventoryItems);
        }

        public void ReloadItems(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: // если добавление
                    IList<InventoryItem> newItems = e.NewItems.Cast<InventoryItem>().ToList();
                    _inventoryPanelSlots.SetAdditionallyData(newItems);
                    break;
                case NotifyCollectionChangedAction.Remove: // если удаление
                    foreach (var items in e.OldItems)
                    {
                        var indexOld = _inventoryPanelSlots.FindIndex((InventoryItem)items);
                        _inventoryPanelSlots.DisableAtIndex(indexOld);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace: // если замена
                    for (var i = 0; i < e.NewItems.Count; i++)
                    {
                        var indexOld = _inventoryPanelSlots.FindIndex((InventoryItem)e.OldItems[i]);
                        _inventoryPanelSlots.ChangeAtIndex((InventoryItem)e.NewItems[i], indexOld);
                    }

                    break;
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