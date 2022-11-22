using System;
using Cards;
using Cards.SituationCards;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    [Serializable]
    public class EventController : BaseController
    {
        [SerializeField] private Text _contents;
        [SerializeField] private Text _eventText;
        //[SerializeField] private Button _customButton;

        public override void Show(LevelCard card)
        {
            var situationCard = (SituationCard)card;
            _contents.text = situationCard.Id;
            _eventText.text = situationCard.Situation.Description;
            
            base.Show(card);
        }
    }
}
