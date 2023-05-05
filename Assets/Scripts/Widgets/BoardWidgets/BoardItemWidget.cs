using System;
using Cards;
using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Utils.Interfaces;

namespace Widgets.BoardWidgets
{
    public sealed class BoardItemWidget : ItemWidget, IClickable<BoardItemWidget>
    {
        [SerializeField] private Image _backGroundImage;
        public Image BackGroundImage => _backGroundImage;
        
        private LevelManager _manager;

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
                if (_manager != null) _manager.ShowEventContainer(_card); //Ну если этот контроллер нужен будет не нулл
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
            _manager = _card.LevelCardType switch
            {
                LevelCardType.Situation=> GameObject.FindWithTag("EventManager").GetComponent<EventManager>(),
                LevelCardType.EndJourney=> GameObject.FindWithTag("EventManager").GetComponent<EventManager>(),
                LevelCardType.Battle => GameObject.FindWithTag("BattleController").GetComponent<BattleEventManager>(),
                _ => _manager
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