using UnityEngine;
using UnityEngine.UI;

namespace Widgets.HUDWidgets
{
    public sealed class PropertyWidget : MonoBehaviour
    {
        [SerializeField] private Text _value;

        public void OnValueChange(int value)
        {
            _value.text = value.ToString();
        }
    }
}