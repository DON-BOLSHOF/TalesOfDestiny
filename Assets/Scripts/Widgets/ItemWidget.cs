using System;
using Cards;
using Cards.SituationCards;
using Controllers;
using UnityEngine;

namespace Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        private LevelCard _card;
        private BaseController _controller;
    
        public void SetData(LevelCard card)
        {
            _card = card;
        }
        public void Click()
        {
            LevelCard card = default;
            
            if (_card.Type == CardType.Situation)
            {
                _controller = FindObjectOfType<EventController>();
                card = (SituationCard)_card;
            }
            
            _controller.Show(card);
        }
    }
}
