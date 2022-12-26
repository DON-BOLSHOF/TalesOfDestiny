using UnityEngine;

namespace View.EndJourneyView
{
    [CreateAssetMenu(menuName = "CardView/EndJourneyCardView", fileName = "EndJourneyCardView")]
    public class EndJourneyCardView : CardView
    {
        [SerializeField] private Vector3 _iconOffset;
        [SerializeField] private string _wisecrack;
        
        public Vector3 IconOffset => _iconOffset;
        public string Wisecrack => _wisecrack;
    }
}