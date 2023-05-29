using UI.View;
using UnityEngine;
using Utils.Disposables;
using Zenject;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private PropertyView _propertyView;

        [Inject] private GameSession _gameSession;
        private DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _trash.Retain(_gameSession.Data.PropertyData.Food.SubscribeAndInvoke(_propertyView.FoodValue.OnValueChange));
            _trash.Retain(
                _gameSession.Data.PropertyData.Prestige.SubscribeAndInvoke(_propertyView.PrestigeValue.OnValueChange));
            _trash.Retain(_gameSession.Data.PropertyData.Coins.SubscribeAndInvoke(_propertyView.CoinValue.OnValueChange));
            _trash.Retain(_gameSession.LevelTurn.SubscribeAndInvoke(_propertyView.TurnValue.OnValueChange));
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}