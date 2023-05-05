using Cards;
using UnityEngine;
using View;

namespace Widgets.BoardWidgets
{
    public abstract class ItemWidget : MonoBehaviour
    {
        protected CardViewWidget _view;
        protected LevelCard _card;

        public CardViewWidget View => _view;
        
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