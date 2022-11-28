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
    public class EventController : BaseController
    {
        [SerializeField] private Text _contents;
        [SerializeField] private Text _eventText;
        [SerializeField] private CustomButtonWidget _customButtonPrefab;
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private PhysicalCard _physicalCard;

        private DataGroup<CustomButtonWidget, CustomButton> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<CustomButtonWidget, CustomButton>(_customButtonPrefab,_buttonContainer);
        }

        public override void Show(LevelCard card)
        {
            var situationCard = (SituationCard)card;
            _contents.text = situationCard.Id;
            _eventText.text = situationCard.Situation.Description;
            _dataGroup.SetData(situationCard.Situation.Buttons);
            _physicalCard.CardViewWidget.SetWidgetData(situationCard.CardView);
            
            base.Show(card);
        }
    }
}
