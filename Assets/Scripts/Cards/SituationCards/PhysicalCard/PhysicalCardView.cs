using UnityEngine;

namespace Cards.SituationCards.PhysicalCard
{
    [CreateAssetMenu(menuName = "CardView/PhysicalCardView", fileName = "PhysicalCardView")]
    public class PhysicalCardView : CardView
    {
        [SerializeField] private Sprite[] _propertyIcons;

        [SerializeField] private string _wisecrack;

        public Sprite[] PropertyIcons => _propertyIcons;
        public string Wisecrack => _wisecrack;
    }
}