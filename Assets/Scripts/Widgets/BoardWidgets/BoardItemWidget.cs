using System;
using Cards;
using UnityEngine;
using UnityEngine.UI;
using Utils.Interfaces;
using Zenject;

namespace Widgets.BoardWidgets
{
    public sealed class BoardItemWidget : ItemWidget, IClickable<BoardItemWidget>
    {
        [SerializeField] private Image _backGroundImage;

        [Inject] private BoardClickHandler _boardClickHandler;

        public Image BackGroundImage => _backGroundImage;
        
        private Animator _viewAnimator;
        private bool _clicked;

        public event Action<BoardItemWidget> IClicked;
        public event Action OnClick;

        private static readonly int Swap = Animator.StringToHash("Swap");

        public void Click()
        {
            DynamicalInitialization();

            var isEndJourney = CheckEndJourneyType(); // Ну в принципе условность игровая - из уровня всегда должна быть
                                                      // возможность уйти.
            
            if (!_clicked || isEndJourney)
            {
                _viewAnimator.SetTrigger(Swap);
                _boardClickHandler.Click(_card);
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
            _viewAnimator = _view.GetComponent<Animator>();
            if (_viewAnimator == null) throw new ArgumentException("View hasn't got an animator!!!");
        }

        private void OnEnable()
        {
            _clicked = false;
        }
    }
}