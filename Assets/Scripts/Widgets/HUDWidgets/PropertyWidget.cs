using CodeAnimation;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets.HUDWidgets
{
    public sealed class PropertyWidget : MonoBehaviour
    {
        [SerializeField] private Text _value;

        [SerializeField] private Graphic _ScalingProperty;
        [SerializeField] private float _scaleFactor;

        public void SetBaseValue(int value)
        {
            _value.text = value.ToString();
        }

        public void OnValueChange(int value)
        {
            StartCoroutine(UpscaleAnimation.ScaleGraphicElement(_ScalingProperty, 1, _scaleFactor,
                () => _value.text = value.ToString()));
        }
    }
}