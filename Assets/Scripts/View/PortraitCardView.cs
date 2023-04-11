using UnityEngine;

namespace View
{
    public abstract class PortraitCardView : CardView
    {
        [SerializeField] private Sprite _mainView;
        [SerializeField] private Vector3 _iconOffset;
        [SerializeField] private Vector3 _portraitCorrection = new Vector3(1,1,1);//Частная коррекция, в виджете общая

        public Sprite MainView => _mainView;
        public Vector3 IconOffset => _iconOffset;
        public Vector3 PortraitCorrection => _portraitCorrection;
    }
}