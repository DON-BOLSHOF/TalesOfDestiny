using System;
using UnityEngine;

namespace Panels
{
    public class InventoryPanel : AbstractPanelUtil
    {
        [SerializeField] private float _approximation;

        private float _baseApproximation;
        
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null) throw new ArgumentException("Camera not found");

            _baseApproximation = _mainCamera.fieldOfView;
        }

        public override void Show()
        {
            OnChangeState?.Invoke(true);

            _mainCamera.fieldOfView = _approximation;
        }

        public override void Exit()
        {
            OnChangeState?.Invoke(false);

            _mainCamera.fieldOfView = _baseApproximation;
        }
    }
}