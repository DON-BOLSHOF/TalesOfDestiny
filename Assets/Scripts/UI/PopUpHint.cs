using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Disposables;
using Timer = Utils.Timer;

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
        }

        private IEnumerator Disappear(Graphic[] graphics)
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