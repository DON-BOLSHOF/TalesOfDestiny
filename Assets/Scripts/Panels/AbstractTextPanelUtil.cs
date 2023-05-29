using System.Collections;
using Cards.SituationCards;
using CodeAnimation;
using Model;
using Model.Data.ControllersData;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Interfaces;

namespace Panels
{
    public abstract class AbstractTextPanelUtil: AbstractPanelUtil, ITypingText, IControllerInteractionVisitor
    {
        [field: SerializeField] public Text[] _texts { get; private set; } //Ебать я гени''й (Как в ребусе)
        [field: SerializeField] public AudioClip _typingClip { get; private set; }
        public TypingAnimation _typingAnimation { get; private set; }

        protected Coroutine _typingRoutine;
        
        public readonly ReactiveEvent<CustomButton[]> OnReloadButtons = new ReactiveEvent<CustomButton[]>();//В панели в любом случае будут кнопки(по крайней мере выхода)

        private void Start()
        {
            var _sfxSource = AudioUtils.FindSfxSource();

            _typingAnimation = new TypingAnimation(_sfxSource, _typingClip, _texts);
        }

        public void OnSkipText()
        {
            if (_typingRoutine == null) return;
            StopCoroutine(_typingRoutine);
            _typingRoutine = null;

            _typingAnimation.SkipText();
        }

        protected abstract void ReloadRandomlySituation(Situation[] situations);

        protected void ReloadStrings(string[] strings)
        {
            _typingAnimation.HideText();
            _typingAnimation.SetStrings(strings);
            StartRoutine(_typingAnimation.TypeText(), ref _typingRoutine);
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

        public void Visit(IControllerInteraction interaction)
        {
            if ((interaction.ControllerType & ControllerInteractionType.Continue) == ControllerInteractionType.Continue)
                ReloadRandomlySituation(interaction.ReactionSituations);
            if ((interaction.ControllerType & ControllerInteractionType.ClosePanel) == ControllerInteractionType.ClosePanel)
                Exit();
        }
    }
}