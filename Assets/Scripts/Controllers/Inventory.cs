using System;
using System.Linq;
using Definitions.Inventory;
using LevelManipulation;
using Model.Data.StorageData;
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
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _inventoryPanel.OnUseItem += OnUsedItem;
                return new ActionDisposable(() => _inventoryPanel.OnUseItem -= OnUsedItem);
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

        private void OnUsedItem(InventoryItem value)
        {
            var propertyBuffs = value.Buffs.Where(t =>
                (t.DataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor);
            foreach (var propertyBuff in propertyBuffs)
            {
                _gameSession.Data.Visit(propertyBuff);
            }
        }

        private void OnDisabledItem(InventoryItem value)
        {
            _gameSession.Data.InventoryData.Visit(this, value);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}