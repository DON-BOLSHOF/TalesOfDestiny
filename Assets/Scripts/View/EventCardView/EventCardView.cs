using UnityEngine;

namespace View.EventCardView
{
    [CreateAssetMenu(menuName = "CardView/EventCardView", fileName = "EventCardView")]
    public class EventCardView : CardView
    {
        [SerializeField] private Vector3 _iconOffset;
        [SerializeField] private Sprite[] _propertyIcons;

        [SerializeField] private string _wisecrack;

        public Vector3 IconOffset => _iconOffset;
        public Sprite[] PropertyIcons => _propertyIcons;
        public string Wisecrack => _wisecrack;
    }
}