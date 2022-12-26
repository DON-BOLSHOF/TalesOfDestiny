using Cards.SituationCards;
using Cards.SituationCards.Event;
using LevelManipulation;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{ 
    public class CustomButtonWidget : MonoBehaviour, IItemInstance<CustomButton>
    {
        [SerializeField] private Text _text;
        [SerializeField] private PanelUtil _panelUtil;

        private ButtonInteraction _interaction;

        public void SetData(CustomButton data)
        {
            _text.text = data.Name;
            _interaction = data.ButtonInteraction;
        }

        public void OnClick()
        {
            _interaction.SetPlayerData(FindObjectOfType<GameSession>().Data).OnClick();
            _interaction.SetPanelButton(_panelUtil).OnClick();
            _interaction.SetLevelManagerButton(FindObjectOfType<LevelManager>()).OnClick();
        }
    }
}