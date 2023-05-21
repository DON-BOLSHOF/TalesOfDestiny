using Cards;
using Controllers;
using UnityEngine;
using View;
using Zenject;

namespace Widgets.BoardWidgets
{
    public class ItemWidget : MonoBehaviour
    {
        protected CardViewWidget _view;
        protected LevelCard _card;

        public CardViewWidget View => _view;
        public LevelCard LevelCard => _card;
        
        private void SetViewData(CardView cardView)
        {
            _view = GetComponentInChildren<CardViewWidget>();
            _view.SetViewData(cardView);
        }

        public void SetData(LevelCard card)
        {
            _card = card;
            SetViewData(card.View);
        }
    }
}