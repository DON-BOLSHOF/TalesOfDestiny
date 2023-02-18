using System;
using System.Collections;
using Cards.SituationCards;
using Cards.SituationCards.Event;
using CodeAnimation;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Interfaces;
using EventType = Cards.SituationCards.Event.EventType;

namespace Panels
{
    public abstract class AbstractTextPanelUtil: AbstractPanelUtil, ITypingText, ICustomButtonVisitor
    {
        [field: SerializeField] public Text[] _texts { get; private set; } //Ебать я гени''й (Как в ребусе)
        [field: SerializeField] public AudioClip _typingClip { get; private set; }
        public TypingAnimation _typingAnimation { get; private set; }

        protected Coroutine _typingRoutine;
        
        public Action<CustomButton[]> OnReloadButtons;//В панели в любом случае будут кнопки(по крайней мере выхода)

        private void Start()
        {
            var _sfxSource = AudioUtils.FindSfxSource();

            _typingAnimation = new TypingAnimation(_sfxSource, _typingClip, _texts);
        }

        protected void StartRoutine(IEnumerator routine, ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            coroutine = StartCoroutine(routine);
        }

        public void OnSkipText()
        {
            if (_typingRoutine == null) return;
            StopCoroutine(_typingRoutine);
            _typingRoutine = null;

            _typingAnimation.SkipText();
        }

        public abstract void ReloadSituation(Situation situation);

        protected void UpdateStrings()
        {
            
        }

        protected void ReloadStrings(string[] strings)
        {
            _typingAnimation.HideText();
            _typingAnimation.SetStrings(strings);
            StartRoutine(_typingAnimation.TypeText(), ref _typingRoutine);
        }

        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.Continue) == EventType.Continue)
                ReloadSituation(interaction.FutureSituation);
            if ((interaction.Type & EventType.ClosePanel) == EventType.ClosePanel)
                Exit();
        }
    }
}