using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using CodeAnimation;
using Definitions.Inventory;
using UI;
using UnityEngine;
using Utils;
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
        
        private static readonly int Showing = Animator.StringToHash("Showing");
        private static readonly int Exiting = Animator.StringToHash("Exiting");

        private void Awake()
        {
            _inventoryPanelItems = new WidgetCollection<InventoryItemWidget, InventoryItem>(_itemsContainer);
            
            _mainCamera = Camera.main;
            if (_mainCamera == null) throw new ArgumentException("Camera not found");
            _baseApproximation = _mainCamera.fieldOfView;

            _animator = GetComponent<Animator>();
            _cameraAnimations = new CameraAnimation(_baseApproximation, _approximation);
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

        public void InitializeItems(List<InventoryItem> inventoryItems)
        {
            _inventoryPanelItems.ReloadData(inventoryItems);
        }
        
        public void ReloadItems(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: // если добавление
                    _inventoryPanelItems.SetAdditionallyData((IList<InventoryItem>)e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove: // если удаление
                    foreach (var items in e.OldItems)
                    {
                        var indexOld = _inventoryPanelItems.FindIndex((InventoryItem)items);
                        _inventoryPanelItems.DisableAtIndex(indexOld);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace: // если замена
                    for(var i = 0; i< e.NewItems.Count; i++)
                    {
                        var indexOld = _inventoryPanelItems.FindIndex((InventoryItem)e.OldItems[i]);
                        _inventoryPanelItems.ChangeAtIndex((InventoryItem)e.NewItems[i],indexOld);
                    }
                    break;
                default:
                    throw new ApplicationException("InventoryPanel: Something unexpected has been done");
                    break;
            }
        }
    }
}