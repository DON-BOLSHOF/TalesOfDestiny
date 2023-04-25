using Controllers;
using Model.Properties;
using Utils.Interfaces;

namespace Utils
{
    public class GameStateAnalyzer : IBattleControllerVisitor, IEventManagerVisitor, IInventoryVisitor
    {
        public ObservableProperty<GameState> GameState { get; }

        public GameStateAnalyzer(GameState gameState)
        {
            GameState = new ObservableProperty<GameState>(gameState);
        }

        public void Visit(BattleController battleController) //Базовая конфигурация
        {
            GameState.Value = Utils.GameState.Battle;
        }

        public void Visit(EventManager eventManagerController) //Базовая конфигурация
        {
            GameState.Value = Utils.GameState.Event;
        }

        public void Visit(Inventory inventory) //Базовая конфигурация
        {
            GameState.Value = Utils.GameState.Inventory;
        }

        public void Visit(BattleController battleController, Stage stage) // Аналог метода с доп. параметрами
        {
            GameState.Value = stage == Stage.End ? Utils.GameState.None : Utils.GameState.Battle;
        }

        public void Visit(EventManager eventManagerController, Stage stage) // Аналог метода с доп. параметрами
        {
            GameState.Value = stage == Stage.End ? Utils.GameState.None : Utils.GameState.Event;
        }

        public void Visit(Inventory inventory, Stage stage)
        {
            GameState.Value = stage == Stage.End ? Utils.GameState.None : Utils.GameState.Inventory; // Аналог метода с доп. параметрами
        }
    }

    public enum Stage
    {
        Start,
        End
    }

    public enum GameState
    {
        None,
        Battle,
        Event,
        Inventory,
        Camp
    }
}