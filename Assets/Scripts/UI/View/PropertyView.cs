using UnityEngine;
using Widgets;

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
    }
}