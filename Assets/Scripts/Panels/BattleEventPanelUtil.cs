using System.Linq;
using Cards.SituationCards;
using CodeAnimation;
using UnityEngine;
using Utils.Interfaces;
using Random = UnityEngine.Random;

namespace Panels
{
    public class BattleEventPanelUtil : AbstractTextPanelUtil, IDissolving //Чисто за анимацию будешь отвечать другалек
    {
        [field: SerializeField] public DissolveAnimation DissolveAnimation { get; private set; }
        [SerializeField] private Animator _animator;

        private Coroutine _shaderRoutine;

        private static readonly int ExitKey = Animator.StringToHash("Exit");
        private static readonly int PrepareBattle = Animator.StringToHash("PrepareBattle");

        public override void Show()
        {
            OnChangeState?.Invoke(true);
            DissolveAnimation.SetActiveDissolve();
            _typingAnimation.HideText();

            StartRoutine(_typingAnimation.TypeText(), ref _typingRoutine);
        }

        public override void Exit()
        {
            OnSkipText();
            _typingAnimation.HideText();

            _animator.SetTrigger(ExitKey);
            OnChangeState?.Invoke(false);
        }

        public void PrepareToBattle()
        {
            OnSkipText();
            _typingAnimation.HideText();
            _animator.SetTrigger(PrepareBattle);
            StartRoutine(DissolveAnimation.EndAnimation(), ref _shaderRoutine);
        }

        public void OnExited() //В аниматоре вызовется
        {
            gameObject.SetActive(false);
        }

        protected override void ReloadRandomlySituation(Situation[] situations) //Пригодится может, иначе удали!
        {
            var currentSituation = situations.ElementAt(Random.Range(0, situations.Length));

            ReloadStrings(new[]
                { currentSituation.SituationName, currentSituation.Description });
            OnReloadButtons?.Invoke(currentSituation.Buttons);
        }
    }
}