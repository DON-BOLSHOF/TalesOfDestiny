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
            
            _trash.Retain(_gameSession.GameStateAnalyzer.GameState.Subscribe(_inventoryPanel.ForceBlockHintExit));
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _inventoryPanel.OnDisableItem += OnDisabledItem;
                return new ActionDisposable(() => _inventoryPanel.OnDisableItem -= OnDisabledItem);
            })());
        }

        public void Show()
        {
            if (_gameSession.GameStateAnalyzer.GameState.Value != GameState.None) 
            {
                _inventoryPanel.ShowBlockHint(); 
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

        private void OnDisabledItem(int index)
        {
            _gameSession.Data.InventoryData.Visit(this, index);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}