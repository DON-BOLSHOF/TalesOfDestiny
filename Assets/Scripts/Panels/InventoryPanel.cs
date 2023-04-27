using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using CodeAnimation;
using Definitions.Inventory;
using UI;
using UnityEngine;
using Utils;

namespace Panels
{
    [RequireComponent(typeof(Animator))]
    public class InventoryPanel : AbstractPanelUtil
    {
        [SerializeField] private float _approximation;

        [SerializeField] private PopUpHint _hint;
        
        private float _baseApproximation;
        private List<InventoryItem> _inventoryPanelItems = new List<InventoryItem>();

        private Camera _mainCamera;

        private Animator _animator;

        private CameraAnimation _cameraAnimations;
        private Coroutine _routine;
        
        private static readonly int Showing = Animator.StringToHash("Showing");
        private static readonly int Exiting = Animator.StringToHash("Exiting");

        private void Awake()
        {
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

        public void ShowHint()
        {
            _hint.Show();
        }

        public void ForceHintExit(GameState state)
        {
            if(state == GameState.None && _hint.IsTimerRunning) _hint.ForceExit();
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(coroutine);
        }

        public void InitializeItems(List<InventoryItem> inventoryItems)
        {
            _inventoryPanelItems = inventoryItems;
        }
        
        public void ReloadItems(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: // если добавление
                    _inventoryPanelItems.AddRange((IEnumerable<InventoryItem>)e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove: // если удаление
                    _inventoryPanelItems.RemoveAt(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace: // если замена
                    for(var i = 0; i< e.NewItems.Count; i++){
                        var inventoryItem = _inventoryPanelItems.FindIndex((Predicate<InventoryItem>)e.OldItems[i]);
                        var item = e.NewItems[i] as InventoryItem;
                        ReloadItem(item, inventoryItem);
                    }
                    break;
            }
        }

        private void ReloadItem(InventoryItem newItem, int inventoryItem)
        {
            _inventoryPanelItems[inventoryItem] = newItem;
        }
    }
}