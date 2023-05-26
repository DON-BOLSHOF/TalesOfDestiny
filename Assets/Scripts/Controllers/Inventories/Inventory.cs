using System;
using System.Linq;
using Definitions.Inventory;
using LevelManipulation;
using Model.Data.StorageData;
using Panels;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Utils.Interfaces.Visitors;
using Zenject;

namespace Controllers.Inventories
{
    public class Inventory : AbstractPanelUtil, IGameStateVisitor
    {
        [SerializeField] private InventoryPanel _inventoryPanel;

        [Inject] private GameSession _gameSession;
        [Inject] private EventLevelBoard _eventLevelBoard;

        private readonly DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _inventoryPanel.InitializeSlots(_gameSession.Data.InventoryData.InventoryItems.ToList());

            _trash.Retain(
                new Func<IDisposable>(() =>
                {
                    _gameSession.Data.InventoryData.InventoryItems.CollectionChanged += _inventoryPanel.ReloadItems;
                    return new ActionDisposable(() =>
                        _gameSession.Data.InventoryData.InventoryItems.CollectionChanged -=
                            _inventoryPanel.ReloadItems);
                })());
            _trash.Retain(_inventoryPanel.OnUseItem.Subscribe(OnUsedItem));
            _trash.Retain(_inventoryPanel.OnThrownOut.Subscribe(OnThrownOut));
        }

        private void OnUsedItem(InventoryItem value)
        {
            var propertyBuffs = value.Buffs.Where(t =>
                (t.DataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor);
            foreach (var propertyBuff in propertyBuffs)
            {
                _gameSession.Data.Visit(propertyBuff);
            }
            
            OnThrownOut(value);
        }

        private void OnThrownOut(InventoryItem value)
        {
            _gameSession.Data.InventoryData.Visit(this, value);
        }

        public override void Show()
        {
            if (!_gameSession.GameStateAnalyzer.TryChangeState(this)) return;
            
            _eventLevelBoard.PrepareCardsField();
            _inventoryPanel.Show();
        }

        public override void Exit()
        {
            _inventoryPanel.Exit();
            _eventLevelBoard.ReturnCardsField();
            VisitGameState(_gameSession.GameStateAnalyzer, Stage.End);
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