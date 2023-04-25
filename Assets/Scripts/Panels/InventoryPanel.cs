using System;
using System.Collections;
using CodeAnimation;
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
    }
}