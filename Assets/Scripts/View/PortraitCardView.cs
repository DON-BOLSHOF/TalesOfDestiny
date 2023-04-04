using UnityEngine;

namespace View
{
    public abstract class PortraitCardView : CardView
    {
        [SerializeField] private Sprite _mainView;
        [SerializeField] private Vector3 _iconOffset;

        public Sprite MainView => _mainView;
        public Vector3 IconOffset => _iconOffset;
    }
}