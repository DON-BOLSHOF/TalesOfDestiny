using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{
    public sealed class PropertyWidget : MonoBehaviour
    {
        [SerializeField] private Text _value;

        public void OnValueChange(float value)
        {
            _value.text = value.ToString();
        }
    }
}