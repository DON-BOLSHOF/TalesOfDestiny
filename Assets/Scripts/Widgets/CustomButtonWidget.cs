using Cards.SituationCards;
using Cards.SituationCards.Event;
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
        [SerializeField] private AbstractPanelUtil _panelUtil;

        private ButtonInteraction _interaction;

        public void SetData(CustomButton pack)
        {
            _text.text = pack.Name;
            _interaction = pack.ButtonInteraction;
        }

        public void OnClick()
        {
            _interaction.SetPlayerData(FindObjectOfType<GameSession>().Data).OnClick();
            _interaction.SetPanelButton(_panelUtil).OnClick();
            _interaction.SetLevelBoardButton(FindObjectOfType<LevelBoard>()).OnClick();
        }
    }
}