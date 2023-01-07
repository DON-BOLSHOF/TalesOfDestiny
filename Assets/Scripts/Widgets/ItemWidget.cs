using Cards;
using UnityEngine;
using View;

namespace Widgets
{
    public abstract class ItemWidget : MonoBehaviour
    {
        protected ItemWidgetView _view;
        protected LevelCard _card;

        public ItemWidgetView View => _view;
        
        private void SetViewData(CardView cardView)
        {
            _view = GetComponentInChildren<ItemWidgetView>();
            _view.SetViewData(cardView);
        }

        public void SetData(LevelCard card)
        {
            _card = card;
            SetViewData(card.View);
        }
    }
}