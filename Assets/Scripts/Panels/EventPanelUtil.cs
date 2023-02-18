using System.Collections;
using System.Collections.Generic;
using Cards.SituationCards;
using CodeAnimation;
using UnityEngine;
using UnityEngine.UI;
using Widgets;

namespace Panels
{
    public class EventPanelUtil : AbstractTextPanelUtil
    {
        [SerializeField] private OutLineAnimation _outLineAnimation;
        [SerializeField] private DissolveAnimation _dissolveAnimation;

        private Coroutine _shaderRoutine;

        public DissolveAnimation DissolveAnimation => _dissolveAnimation;
        public OutLineAnimation OutLineAnimation => _outLineAnimation;

        public override void Show()
        {
            _typingAnimation.TakeText();
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

        public override void
            ReloadSituation(Situation situation) //Код начинает в спагетти превращаться с кусочками говна...
        {
            var graphics = FindButtonWidgets();

            StartRoutine(
                _dissolveAnimation.ReloadSpecialObjects(graphics, () => ReloadButton(situation.Buttons),
                    FindButtonWidgets), ref _shaderRoutine); //Это или слишком тупо или гениально
            ReloadStrings(new[]
                { situation.name, situation.Description });
        }

        private List<Graphic> FindButtonWidgets()
        {
            var widgets = GetComponentsInChildren<CustomButtonWidget>();
            var graphics = new List<Graphic>();

            foreach (var widget in widgets)
            {
                graphics.AddRange(widget.GetComponentsInChildren<Graphic>());
            }

            return graphics;
        }

        private int ReloadButton(CustomButton[] buttons)
        {
            OnReloadButtons.Invoke(buttons);

            return 0; //Сделано лишь потому что Func<void> не берется... Нет времени щас разбираться, потом
        }
    }
}