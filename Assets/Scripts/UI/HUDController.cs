using System;
using Panels;
using UI.View;
using UnityEngine;
using Utils.Disposables;
using Zenject;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private PropertyView _propertyView;
        [SerializeField] private NotificationPanel _notificationPanel;

        [Inject] private NotificationHandler _notificationHandler;
        
        [Inject] private GameSession _gameSession;
        private DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _propertyView.SetBaseValue(_gameSession.Data.PropertyData.Food.Value, _gameSession.Data.PropertyData.Prestige.Value,
                _gameSession.Data.PropertyData.Coins.Value, _gameSession.LevelTurn.Value);
            
            _trash.Retain(_gameSession.Data.PropertyData.Food.Subscribe(_propertyView.FoodValue.OnValueChange));
            _trash.Retain(_gameSession.Data.PropertyData.Prestige.Subscribe(_propertyView.PrestigeValue.OnValueChange));
            _trash.Retain(_gameSession.Data.PropertyData.Coins.Subscribe(_propertyView.CoinValue.OnValueChange));
            _trash.Retain(new Func<IDisposable>(() =>
                {
                    _gameSession.Data.InventoryData.InventoryItems.CollectionChanged += _notificationHandler.NotifyInventory;
                    return new ActionDisposable(() =>
                        _gameSession.Data.InventoryData.InventoryItems.CollectionChanged -=
                            _notificationHandler.NotifyInventory);
                })());
            _trash.Retain(_notificationHandler.OnNotificationSent.Subscribe(_notificationPanel.OnNotificationSent));
            _trash.Retain(_gameSession.LevelTurn.Subscribe(_propertyView.TurnValue.OnValueChange));
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}