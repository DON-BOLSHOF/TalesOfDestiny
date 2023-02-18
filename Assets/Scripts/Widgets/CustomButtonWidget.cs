using Cards.SituationCards;
using Cards.SituationCards.Event;
using Controllers;
using LevelManipulation;
using Panels;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Widgets
{ 
    public class CustomButtonWidget : MonoBehaviour, IItemInstance<CustomButton>
    {
        [SerializeField] private Text _text;
        [SerializeField] private AbstractTextPanelUtil textPanelUtil;

        private ButtonInteraction _interaction;

        public void SetData(CustomButton pack)
        {
            _text.text = pack.Name;
            _interaction = pack.ButtonInteraction;
        }

        public void OnClick()
        {
            _interaction.SetButtonVisitor(FindObjectOfType<GameSession>().Data).OnClick();//Ну с натяжкой он может знать об этом)))
            _interaction.SetButtonVisitor(textPanelUtil).OnClick();
            _interaction.SetButtonVisitor(FindObjectOfType<LevelBoard>()).OnClick();
            _interaction.SetButtonVisitor(FindObjectOfType<BattleController>()).OnClick();
        }
    }
}