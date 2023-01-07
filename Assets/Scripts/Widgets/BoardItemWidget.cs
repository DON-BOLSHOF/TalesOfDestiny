using System;
using Cards;
using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Utils.Interfaces;

namespace Widgets
{
    public sealed class BoardItemWidget : ItemWidget, IClickable<BoardItemWidget>
    {
        [SerializeField] private Image _backGroundImage;
        public Image BackGroundImage => _backGroundImage;
        
        private LevelController _controller;

        private Animator _viewAnimator;

        public event Action<BoardItemWidget> IClicked;
        public event Action OnClick;

        private bool _clicked;

        private static readonly int Swap = Animator.StringToHash("Swap");

        public void Click()
        {
            DynamicalInitialization();

            var isEndJourney = CheckEndJourneyType(); // Ну в принципе условность игровая - из уровня всегда должна быть
                                                      // возможность уйти.
            
            if (!_clicked || isEndJourney)
            {
                _viewAnimator.SetTrigger(Swap);
                if (_controller != null) _controller.Show(_card); //Ну если этот контроллер нужен будет не нулл
                _clicked = true;
            }

            OnClick?.Invoke();
            IClicked?.Invoke(this);
        }

        private bool CheckEndJourneyType()
        {
            if (_card.Type != CardType.Event) return false;
            return ((EventCard)_card).LevelCardType == LevelCardType.EndJourney;
        }

        private void DynamicalInitialization()
        {
            _controller = _card.Type switch
            {
                CardType.Event => FindObjectOfType<EventController>(),
                CardType.Enemy => FindObjectOfType<BattleController>(),
                _ => _controller
            };

            _viewAnimator = _view.GetComponent<Animator>();
            if (_viewAnimator == null) throw new ArgumentException("View hasn't got an animator!!!");
        }

        private void OnEnable()
        {
            _clicked = false;
        }
    }
}