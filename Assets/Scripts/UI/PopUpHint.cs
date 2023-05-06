using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils.Disposables;
using Timer = Utils.Timer;

namespace UI
{
    public class PopUpHint : MonoBehaviour
    {
        [SerializeField] protected Graphic[] _graphics;
        [SerializeField] private int _popUpTime;

        protected float _currentAlpha;
        protected Coroutine _graphicRoutine;

        protected Timer _timer;
        
        public Action OnShown;
        public Action OnDisappeared;

        private DisposeHolder _trash = new DisposeHolder();

        public bool IsTimerRunning => _timer.IsRunning;

        private void Awake()
        {
            _timer = new Timer(_popUpTime);
            _trash.Retain(_timer.Subscribe(Exit));
        }

        public virtual void Show()
        {
            StartRoutine(Show(_graphics), ref _graphicRoutine);
            _timer.ReloadTimer();
        }

        public virtual void ForceExit()
        {
            _timer.StopTimer();
            Exit();
        }

        protected virtual void Exit()
        {
            StartRoutine(Disappear(_graphics), ref _graphicRoutine);
        }

        protected virtual IEnumerator Show(Graphic[] graphics)
        {
            for (; _currentAlpha <= 1.05f; _currentAlpha += 0.05f)
            {
                foreach (var graphic in graphics)
                {
                    var variableColor = graphic.color;
                    variableColor.a = _currentAlpha;
                    graphic.color = variableColor;
                }

                yield return new WaitForSeconds(0.07f);
            }

            _currentAlpha = 1;
            OnShown?.Invoke();
        }

        protected virtual IEnumerator Disappear(Graphic[] graphics)
        {
            for (; _currentAlpha >= -0.05; _currentAlpha -= 0.05f)
            {
                foreach (var graphic in graphics)
                {
                    var variableColor = graphic.color;
                    variableColor.a = _currentAlpha;
                    graphic.color = variableColor;
                }

                yield return new WaitForSeconds(0.07f);
            }

            _currentAlpha = 0;
            OnDisappeared?.Invoke();
        }

        protected void ChangeGraphicsAlphaTo(float value)
        {
            _currentAlpha = value;
            foreach (var graphic in _graphics)
            {
                var variableColor = graphic.color;
                variableColor.a = _currentAlpha;
                graphic.color = variableColor;
            }
        }
        
        public void ReloadTimer()
        {
            _timer?.ReloadTimer();
        }

        public void StopTimer()
        {
            _timer?.StopTimer();
        }
        
        public IDisposable SubscribeOnShown(Action action)
        {
            OnShown += action;
            return new ActionDisposable(() => OnShown -= action);
        }
        
        public IDisposable SubscribeOnDisappeared(Action action)
        {
            OnDisappeared += action;
            return new ActionDisposable(() => OnDisappeared -= action);
        }

        protected void StartRoutine(IEnumerator coroutine, ref Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);

            routine = StartCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

#if UNITY_EDITOR

        [Button(ButtonSizes.Small)]
        private void GetGraphics()
        {
            _graphics = GetComponentsInChildren<Graphic>();
        }

        [Button(ButtonSizes.Small)]
        private void AlphaToZero()
        {
            foreach (var graphic in _graphics)
            {
                var variableColor = graphic.color;
                variableColor.a = 0;
                graphic.color = variableColor;
            }
        }

        [Button(ButtonSizes.Small)]
        private void AlphaToOne()
        {
            foreach (var graphic in _graphics)
            {
                var variableColor = graphic.color;
                variableColor.a = 1;
                graphic.color = variableColor;
            }
        }
#endif
    }
}