using UnityEngine;
using View;
using View.EventCardView;
using Widgets;

namespace Cards.SituationCards.EventCard
{
    public class EventCard : MonoBehaviour
    {
        [SerializeField] private EventCardViewWidget eventCardViewWidget;

        public EventCardViewWidget EventCardViewWidget => eventCardViewWidget;
    }
}
