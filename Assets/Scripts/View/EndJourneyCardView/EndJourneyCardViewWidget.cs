using UnityEngine;
using UnityEngine.UI;

namespace View.EndJourneyCardView
{
    public sealed class EndJourneyCardViewWidget : CardViewWidget
    {
        [SerializeField] private Image _background;
        
         public override void SetViewData(CardView cardView)
         {
             _background.sprite = cardView.BackgroundView;
         }
    }
}