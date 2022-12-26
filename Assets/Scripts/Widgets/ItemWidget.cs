using Cards;
using Controllers;
using UnityEngine;
using View;

namespace Widgets
{
    public sealed class ItemWidget : MonoBehaviour
    {
        private LevelCard _card;
        private LevelController _controller;

        private ItemWidgetView _view;
        private Animator _viewAnimator;
        
        private static readonly int Swap = Animator.StringToHash("Swap");

        private void SetViewData(CardView cardView)
        {
            _view = GetComponentInChildren<ItemWidgetView>();
            _viewAnimator = _view.GetComponent<Animator>();
            _view.SetViewData(cardView);
        }

        public void SetData(LevelCard card)
        {
            _card = card;
            SetViewData(card.View);
        }

        public void Click()
        {
            if (_card.Type == CardType.Situation)
            {
                _controller = FindObjectOfType<EventController>();
            }

            _viewAnimator.SetTrigger(Swap);
            _controller.Show(_card);
        }
    }
}