using System;
using System.Threading;
using UnityEngine;
using Utils.Disposables;

namespace Utils
{
    public class Timer
    {
        public bool IsRunning => _timerState == ActiveTimer.Active;
        public int RemainingSeconds => IsRunning? _totalTime - _currentTime: 0;
        public event Action OnTimeExpired;
    
        private int _totalTime;
        private ActiveTimer _timerState = ActiveTimer.Deactive;
        private CancellationTokenSource _cancellationTokenSource;
        private int _currentTime;

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
            _currentTime = 0;

            while (_currentTime < _totalTime)
            {
                await new WaitForSecondsRealtime(1);
                if (token.IsCancellationRequested) return;
                _currentTime++;
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
}