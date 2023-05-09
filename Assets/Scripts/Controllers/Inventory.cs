using System;
using System.Linq;
using Definitions.Inventory;
using LevelManipulation;
using Model.Data.StorageData;
using Panels;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Utils.Interfaces;
using Zenject;

namespace Controllers
{
    public class Inventory : MonoBehaviour, IGameStateVisitor
    {
        [SerializeField] private InventoryPanel _inventoryPanel;

        [Inject] private GameSession _gameSession;
        [Inject] private EventLevelBoard _eventLevelBoard;

        private readonly DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _inventoryPanel.InitializeItems(_gameSession.Data.InventoryData.InventoryItems.ToList());

            _trash.Retain(new Func<IDisposable>(() =>
            {
                _inventoryPanel.OnUseItem += OnUsedItem;
                return new ActionDisposable(() => _inventoryPanel.OnUseItem -= OnUsedItem);
            })());
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _inventoryPanel.OnDisableItem += OnDisabledItem;
                return new ActionDisposable(() => _inventoryPanel.OnDisableItem -= OnDisabledItem);
            })()); 
        }

        public void Show()
        {
            if (!_gameSession.GameStateAnalyzer.TryChangeState(this)) return;
            
            _eventLevelBoard.PrepareCardsField();
            _inventoryPanel.Show();
        }


        private void OnUsedItem(InventoryItem value)
        {
            var propertyBuffs = value.Buffs.Where(t =>
                (t.DataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor);
            foreach (var propertyBuff in propertyBuffs)
            {
                _gameSession.Data.Visit(propertyBuff);
            }
        }

        public void Exit()
        {
            _inventoryPanel.Exit();
            _eventLevelBoard.ReturnCardsField();
            VisitGameState(_gameSession.GameStateAnalyzer, Stage.End);
        }

        private void OnDisabledItem(InventoryItem value)
        {
            _gameSession.Data.InventoryData.Visit(this, value);
        }

        public void VisitGameState(GameStateAnalyzer gameStateAnalyzer, Stage stage)
        {
            gameStateAnalyzer.Visit(this, stage);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}