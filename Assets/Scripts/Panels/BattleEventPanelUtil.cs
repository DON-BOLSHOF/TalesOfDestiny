using Cards.SituationCards;
using CodeAnimation;
using UnityEngine;
using Utils.Interfaces;

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

        public override void ReloadSituation(Situation situation) //Пригодится может, иначе удали!
        {
            ReloadStrings(new[]
                { situation.name, situation.Description });
            OnReloadButtons?.Invoke(situation.Buttons);
        }

        public void OnExited() //В аниматоре вызовется
        {
            gameObject.SetActive(false);
        }

        public void PrepareToBattle()
        {
            OnSkipText();
            _typingAnimation.HideText();
            _animator.SetTrigger(PrepareBattle);
            StartRoutine(DissolveAnimation.Dissolving(), ref _shaderRoutine);
        }
    }
}