using Cards.SituationCards.Event;
using Controllers;
using LevelManipulation;
using Panels;
using Zenject;

namespace Model
{
    public class StorageData//Прокидывает дату куда кому надо.
    {
        [Inject] private GameSession _session;
        [Inject] private EventLevelBoard _levelBoard;
        [Inject] private BattleController _battleController;
        
        public void InteractData(ButtonInteraction interaction) 
        {
            interaction.SetButtonVisitor(_session.Data).OnClick();
            interaction.SetButtonVisitor(_levelBoard).OnClick();
            interaction.SetButtonVisitor(_battleController).OnClick();
        } 
        
        public void InteractData(ButtonInteraction interaction, AbstractTextPanelUtil textPanelUtil) 
        {
            InteractData(interaction);
            interaction.SetButtonVisitor(textPanelUtil).OnClick();
        }
    }
}