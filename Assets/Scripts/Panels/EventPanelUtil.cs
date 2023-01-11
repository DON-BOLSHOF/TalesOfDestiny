using System.Collections;
using CodeAnimation;
using UnityEngine;

namespace Panels
{
    public class EventPanelUtil : AbstractPanelUtil
    {
        [SerializeField] private OutLineAnimation _outLineAnimation;
        [SerializeField] private DissolveAnimation _dissolveAnimation;

        private Coroutine _shaderRoutine;

        public DissolveAnimation DissolveAnimation => _dissolveAnimation;
        public OutLineAnimation OutLineAnimation => _outLineAnimation;

        public override void Show()
        {
            _typingAnimation.HideText();

            _dissolveAnimation.SetImagesDissolve();

            StartRoutine(Showing(), ref _shaderRoutine);

            OnChangeState?.Invoke(true);
        }

        private void Dissolve()
        {
            OnSkipText();

            StartRoutine(OutLiningOff(), ref _shaderRoutine);
        }

        private IEnumerator Showing()
        {
            yield return _dissolveAnimation.Emerging();

            StartRoutine(_outLineAnimation.OutLiningOn(), ref _shaderRoutine);
            StartRoutine(_typingAnimation.TypeText(), ref _typingRoutine);
        }
    
        private IEnumerator Dissolving()
        {
            _typingAnimation.HideText();

            yield return _dissolveAnimation.Dissolving();

            OnChangeState?.Invoke(false);
            gameObject.SetActive(false);
        }

        private IEnumerator OutLiningOff()
        {
            yield return _outLineAnimation.OutLiningOff();

            StartRoutine(Dissolving(), ref _shaderRoutine);
        }

        public override void Exit()
        {
            Dissolve();
        }
    }
}