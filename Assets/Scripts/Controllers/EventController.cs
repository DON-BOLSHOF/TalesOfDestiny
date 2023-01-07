using System;
using Cards;
using Cards.SituationCards;
using Cards.SituationCards.PhysicalCard;
using UnityEngine;
using UnityEngine.UI;
using Widgets;

namespace Controllers
{
    [Serializable]
    public class EventController : LevelController
    {
        [SerializeField] private Text _contents;
        [SerializeField] private Text _eventText;
        [SerializeField] private CustomButtonWidget _customButtonPrefab;
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private PanelUtilItemWidget _panelUtilItemWidget;

        private DataGroup<CustomButtonWidget, CustomButton> _dataGroup;

        protected virtual void Start()
        {
            _dataGroup = new DataGroup<CustomButtonWidget, CustomButton>(_customButtonPrefab,_buttonContainer);
        }

        public override void Show(LevelCard card)
        {
            DynamicInitialization(card);
            
            base.Show(card);
        }

        private void DynamicInitialization(LevelCard card)
        {
            var eventCard = (EventCard)card;
            if (eventCard == null) throw new ArgumentException("Was sent not EventCard!!!");
            
            ItemWidgetFactory.FulFillItemWidget(_panelUtilItemWidget, WidgetType.PanelUtil, card);
            
            _contents.text = eventCard.Id;
            _eventText.text = eventCard.Situation.Description;
            _dataGroup.SetData(eventCard.Situation.Buttons);
            _panelUtilItemWidget.View.SetViewData(eventCard.View);
        }
    }
}
