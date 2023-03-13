using System;
using Cards.SituationCards;
using Cards.SituationCards.Event;
using Controllers;
using LevelManipulation;
using Panels;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Widgets
{ 
    public class CustomButtonWidget : MonoBehaviour, IItemInstance<CustomButton>
    {
        [SerializeField] private Text _text;

        [Inject] private GameSession _session;
        [Inject] private EventLevelBoard _levelBoard;
        [Inject] private BattleController _battleController;
        
        private AbstractTextPanelUtil textPanelUtil;
        private ButtonInteraction _interaction;

        private bool _alreadyClicked;

        private void Start()
        {
            textPanelUtil = GetComponentInParent<AbstractTextPanelUtil>();
        }

        public void SetData(CustomButton pack)
        {
            _text.text = pack.Name;
            _interaction = pack.ButtonInteraction;
        }

        public void OnClick()
        {
            if(_alreadyClicked) return;
            
            _interaction.SetButtonVisitor(_session.Data).OnClick();//Ну с натяжкой он может знать об этом)))
            _interaction.SetButtonVisitor(textPanelUtil).OnClick();
            _interaction.SetButtonVisitor(_levelBoard).OnClick();
            _interaction.SetButtonVisitor(_battleController).OnClick();
            _alreadyClicked = true;
        }

        public void ActivateButton()
        {
            _alreadyClicked = false;
        }
        private void OnDisable()
        {
            ActivateButton();
        }

    }
}