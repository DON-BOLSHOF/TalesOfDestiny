using UnityEngine;
using UnityEngine.UI;
using Utils.Disposables;
using Utils.Interfaces;

namespace LevelManipulation
{
    public class ClickBlocker : MonoBehaviour, ISubscriber
    {
        private Image _image;
        private DisposeHolder _trash = new DisposeHolder();

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.raycastTarget = false;
        }

        private void Activate(bool value)
        {
            _image.raycastTarget = value;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public void Subscribe()
        {
            var subscriber = GetComponentInParent<LevelBoardSubscriber>();
            _trash.Retain(subscriber.Subscribe(Activate));
        }
    }
}