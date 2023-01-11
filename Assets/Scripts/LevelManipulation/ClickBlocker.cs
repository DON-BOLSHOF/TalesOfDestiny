using Panels;
using UnityEngine;
using UnityEngine.UI;

namespace LevelManipulation
{
    public class ClickBlocker : MonoBehaviour
    {
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            _image.raycastTarget = false;

            var checks = Resources.FindObjectsOfTypeAll<AbstractPanelUtil>();
            foreach (var check in checks)
            {
                check.OnChangeState += Activate;
            }
        }

        private void Activate(bool value)
        {
            _image.raycastTarget = value;
        }
    }
}