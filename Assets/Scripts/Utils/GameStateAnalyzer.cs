using System;
using Controllers;
using Model.Properties;
using Utils.Disposables;
using Utils.Interfaces;

namespace Utils
{
    public class GameStateAnalyzer
    {
        public ObservableProperty<GameState> GameState { get; }

        public event Action OnStateChangeBlocked;

        public GameStateAnalyzer(GameState gameState)
        {
            GameState = new ObservableProperty<GameState>(gameState);
        }
        
        public void Visit(IGameStateVisitor gameStateVisitor, Stage stage)
        {
            if (gameStateVisitor as Inventory)//Херово как-то, но иначе try не сделать...
            {
                GameState.Value = stage == Stage.Start? Utils.GameState.Inventory: Utils.GameState.None;
            }

            if (gameStateVisitor as EventManager)
            {
                GameState.Value = stage == Stage.Start? Utils.GameState.Event: Utils.GameState.None;
            }

            if (gameStateVisitor as BattleController)
            {
                GameState.Value = stage == Stage.Start? Utils.GameState.Battle: Utils.GameState.None;
            }
        }

        public bool TryChangeState(IGameStateVisitor gameStateVisitor)
        {
            if (GameState.Value != Utils.GameState.None)
            {
                OnStateChangeBlocked?.Invoke();
                return false;
            }

            gameStateVisitor.VisitGameState(this, Stage.Start);// Понятно что хотим изменить стейт, поэтому Start
            return true;
        }

        public IDisposable SubscribeOnStateChangeBlocked(Action call)
        {
            OnStateChangeBlocked += call;
            return new ActionDisposable(() => OnStateChangeBlocked -= call);
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