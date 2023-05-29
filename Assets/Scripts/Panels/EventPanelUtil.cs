using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.SituationCards;
using CodeAnimation;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Utils.Interfaces;
using Widgets.EventManagersWidgets;

namespace Panels
{
    public class EventPanelUtil : AbstractTextPanelUtil, IDissolving, IOutLining
    {
        [field: SerializeField] public OutLineAnimation OutLineAnimation { get; private set; }
        [field: SerializeField] public DissolveAnimation DissolveAnimation { get; private set; }

        private Coroutine _shaderRoutine;

        public override void Show()
        {
            _typingAnimation.TakeText();
            _typingAnimation.HideText();

            DissolveAnimation.SetDeactiveDissolve();

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
            yield return DissolveAnimation.StartAnimation();

            StartRoutine(OutLineAnimation.StartAnimation(), ref _shaderRoutine);
            StartRoutine(_typingAnimation.TypeText(), ref _typingRoutine);
        }

        private IEnumerator Dissolving()
        {
            _typingAnimation.HideText();
            DissolveAnimation.SetBaseMaterialsForSpecials();
            
            yield return DissolveAnimation.EndAnimation();

            OnChangeState?.Invoke(false);
            gameObject.SetActive(false);
        }

        private IEnumerator OutLiningOff()
        {
            yield return OutLineAnimation.EndAnimation();

            StartRoutine(Dissolving(), ref _shaderRoutine);
        }

        public override void Exit()
        {
            Dissolve();
        }

        protected override void ReloadRandomlySituation(Situation[] situations) //Код начинает в спагетти превращаться, с кусочками говна...
        {
            var currentSituation = situations.ElementAt(Random.Range(0, situations.Length));

            var buttonWidgets = FindButtonWidgets();

            StartRoutine(
                DissolveAnimation.ReloadSpecialObjects(buttonWidgets, () =>
                    {
                        GetComponentsInChildren<CustomButtonWidget>().ToList().ForEach(x => x.ActivateButton());
                        return ReloadButton(currentSituation.Buttons);
                    },
                    FindButtonWidgets), ref _shaderRoutine); //Это или слишком тупо или гениально
            ReloadStrings(new[] { currentSituation.SituationName, currentSituation.Description });
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
            OnReloadButtons.Execute(buttons);

            return 0; //Сделано лишь потому что Func<void> не берется... Нет времени щас разбираться, потом
        }
    }
}