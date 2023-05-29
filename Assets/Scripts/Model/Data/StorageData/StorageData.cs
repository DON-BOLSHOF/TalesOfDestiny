using Controllers.BattleControllers;
using LevelManipulation;
using Model.Data.ControllersData;
using Model.Tributes;
using Panels;
using Zenject;

namespace Model.Data.StorageData
{
    public class StorageData//Прокидывает дату куда кому надо.
    {
        [Inject] private GameSession _session;
        [Inject] private EventLevelBoard _levelBoard;
        [Inject] private BattleController _battleController;

        public void InteractData(IDataInteraction interaction)
        {
            interaction.Accept(_session.Data);
        }
        
        public void InteractData(IControllerInteraction interaction) 
        {
            interaction.Accept(_levelBoard);
            interaction.Accept(_battleController);
        }

        public void InteractData(IControllerInteraction interaction, AbstractTextPanelUtil textPanelUtil) 
        {
            InteractData(interaction);
            interaction.Accept(textPanelUtil);
        }

        public void InteractData(ITribute tribute)
        {
            tribute.Accept(_session.Data);
        }
    }
}