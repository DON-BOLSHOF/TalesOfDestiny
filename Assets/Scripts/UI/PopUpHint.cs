using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Utils.Disposables;

namespace UI
{
    public class PopUpHint : MonoBehaviour
    {
        [SerializeField] private Graphic[] _graphics;
        [SerializeField] private int _popUpTime;

        private float _currentAlpha;
        private Coroutine _graphicRoutine;
        
        private Timer _timer;

        private DisposeHolder _trash = new DisposeHolder();

        public bool IsTimerRunning => _timer.IsRunning;

        private void Awake()
        {
            _timer = new Timer(_popUpTime);
            _trash.Retain(_timer.Subscribe(Exit));
        }

        public void Show()
        {
            StartRoutine(Show(_graphics), ref _graphicRoutine);
            _timer.ReloadTimer();
        }

        public void ForceExit()
        {
            _timer.StopTimer();
            Exit();
        }

        private void Exit()
        {
            StartRoutine(Disappear(_graphics), ref _graphicRoutine);
        }

        private IEnumerator Show(Graphic[] graphics)
        {
            for (; _currentAlpha <= 1; _currentAlpha += 0.05f)
            {
                foreach (var graphic in graphics)
                {
                    var variableColor = graphic.color;
                    variableColor.a = _currentAlpha;
                    graphic.color = variableColor;
                }

                yield return new WaitForSeconds(0.07f);
            }
        }

        private IEnumerator Disappear(Graphic[] graphics)
        {
            for (; _currentAlpha >= 0; _currentAlpha -= 0.05f)
            {
                foreach (var graphic in graphics)
                {
                    var variableColor = graphic.color;
                    variableColor.a = _currentAlpha;
                    graphic.color = variableColor;
                }
                yield return new WaitForSeconds(0.07f);
            }
        }

        private void StartRoutine(IEnumerator coroutine, ref Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);

            routine = StartCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}

public class Timer
{
    public bool IsRunning => _timerState == ActiveTimer.Active;
    public event Action OnTimeExpired;
    
    private int _totalTime;
    private ActiveTimer _timerState = ActiveTimer.Deactive;
    private CancellationTokenSource _cancellationTokenSource;

    public Timer(int time)
    {
        _totalTime = time;
    }

    public async void ReloadTimer()
    {
        StopTimer();

        _timerState = ActiveTimer.Active;
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        var currentTime = 0;

        while (currentTime < _totalTime)
        {
            await new WaitForSecondsRealtime(1);
            if (token.IsCancellationRequested) return;
            currentTime++;
        }

        OnTimeExpired?.Invoke();
        _timerState = ActiveTimer.Deactive;
    }

    public void StopTimer()
    {
        try
        {
            if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested) return;
            _timerState = ActiveTimer.Deactive;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
        catch (ObjectDisposedException e)
        {
            Debug.LogWarning("The object is already disposed." + e);
        }
    }

    public IDisposable Subscribe(Action action)
    {
        OnTimeExpired += action;
        return new ActionDisposable(() => OnTimeExpired -= action);
    }
    
    private enum ActiveTimer
    {
        Deactive,
        Active
    }
}