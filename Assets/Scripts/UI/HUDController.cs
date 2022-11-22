using UI.View;
using UnityEngine;
using Utils.Disposables;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private PropertyView _propertyView;

        private GameSession _gameSession;
        private DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();

            _trash.Retain(_gameSession.Data.HeroData.Food.SubscribeAndInvoke(_propertyView.FoodValue.OnValueChange));
            _trash.Retain(
                _gameSession.Data.HeroData.Prestige.SubscribeAndInvoke(_propertyView.PrestigeValue.OnValueChange));
            _trash.Retain(_gameSession.Data.HeroData.Coins.SubscribeAndInvoke(_propertyView.CoinValue.OnValueChange));
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}