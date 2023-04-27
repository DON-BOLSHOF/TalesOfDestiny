using System;
using System.Linq;
using LevelManipulation;
using Panels;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Zenject;

namespace Controllers
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private InventoryPanel _inventoryPanel;

        [Inject] private GameSession _gameSession;
        [Inject] private EventLevelBoard _eventLevelBoard;

        private readonly DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _inventoryPanel.InitializeItems(_gameSession.Data.InventoryData.InventoryItems.ToList());
            
            _trash.Retain(
                new Func<IDisposable>(() =>
                {
                    _gameSession.Data.InventoryData.InventoryItems.CollectionChanged += _inventoryPanel.ReloadItems;
                    return new ActionDisposable(() =>
                        _gameSession.Data.InventoryData.InventoryItems.CollectionChanged -=
                            _inventoryPanel.ReloadItems);
                })());
            _trash.Retain(_gameSession.GameStateAnalyzer.GameState.Subscribe(_inventoryPanel.ForceHintExit));
        }

        public void Show()
        {
            if (_gameSession.GameStateAnalyzer.GameState.Value !=
                GameState.None) //Добавить всплывающий хинт для игроков
            {
                _inventoryPanel
                    .ShowHint(); //Сделай потом чтобы подписку на изм стейта, чтобы сразу выключалась при изм стейта на полож
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

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}