using LevelManipulation;
using Panels;
using UnityEngine;
using Utils;
using Zenject;

namespace Controllers
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private InventoryPanel _inventoryPanel;

        [Inject] private GameSession _gameSession;
        [Inject] private EventLevelBoard _eventLevelBoard;

        private void Awake()
        {
            _gameSession.GameStateAnalyzer.GameState.Subscribe(_inventoryPanel.ForceHintExit);
        }

        public void Show()
        {
            if (_gameSession.GameStateAnalyzer.GameState.Value != GameState.None) //Добавить всплывающий хинт для игроков
            {
                _inventoryPanel.ShowHint();//Сделай потом чтобы подписку на изм стейта, чтобы сразу выключалась при изм стейта на полож
            }
            else
            {
                _gameSession.GameStateAnalyzer.Visit(this, Stage.Start);
                _eventLevelBoard.PrepareCardsField();
                _inventoryPanel.Show();
            }
        }

        public void Exit()
        {
            _inventoryPanel.Exit();
            _eventLevelBoard.ReturnCardsField();
            _gameSession.GameStateAnalyzer.Visit(this, Stage.End);
        }
    }
}