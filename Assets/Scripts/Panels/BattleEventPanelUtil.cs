﻿using Cards.SituationCards;
using UnityEngine;

namespace Panels
{
    public class BattleEventPanelUtil : AbstractPanelUtil //Чисто за анимацию будешь отвечать другалек
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int ExitKey = Animator.StringToHash("Exit");

        public override void Show()
        {
            OnChangeState?.Invoke(true);
            _typingAnimation.HideText();
            
            StartRoutine(_typingAnimation.TypeText(), ref _typingRoutine);
        }
        
        public override void Exit()
        {
            OnSkipText();
            _typingAnimation.HideText();
            
            _animator.SetTrigger(ExitKey);
        }

        public override void ReloadSituation(Situation situation)//Пригодится может, иначе удали!
        {
            ReloadStrings(new[]
                { situation.name, situation.Description });
            OnReloadButtons?.Invoke(situation.Buttons);
        }

        public void OnExited()//В аниматоре вызовется
        {
            OnChangeState?.Invoke(false);
            gameObject.SetActive(false);
        }
    }
}