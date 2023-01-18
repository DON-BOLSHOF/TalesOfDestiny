using UnityEngine;

namespace View.SituationCardView
{
    [CreateAssetMenu(menuName = "CardView/EventCardView", fileName = "EventCardView")]
    public class SituationCardView : CardView
    {
        [SerializeField] private Sprite _mainView;
        [SerializeField] private Vector3 _iconOffset;
        [SerializeField] private Sprite[] _propertyIcons;

        [SerializeField] private string _wisecrack;

        public Sprite MainView => _mainView;
        public Vector3 IconOffset => _iconOffset;
        public Sprite[] PropertyIcons => _propertyIcons;
        public string Wisecrack => _wisecrack;
    }
}