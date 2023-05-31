using UnityEngine;
using Widgets;
using Widgets.HUDWidgets;

namespace UI.View
{
    public class PropertyView : MonoBehaviour
    {
        [SerializeField] private PropertyWidget _foodValue;
        [SerializeField] private PropertyWidget _prestigeValue;
        [SerializeField] private PropertyWidget _coinValue;
        [SerializeField] private PropertyWidget _turnValue;

        public PropertyWidget FoodValue => _foodValue;
        public PropertyWidget PrestigeValue => _prestigeValue;
        public PropertyWidget CoinValue => _coinValue;
        public PropertyWidget TurnValue => _turnValue;

        public void SetBaseValue(int foodValue, int prestigeValue, int coinValue, int turnValue)
        {
            _foodValue.SetBaseValue(foodValue);
            _prestigeValue.SetBaseValue(prestigeValue);
            _coinValue.SetBaseValue(coinValue);
            _turnValue.SetBaseValue(turnValue);
        }
    }
}