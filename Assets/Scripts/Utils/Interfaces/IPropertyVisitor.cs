using Cards.SituationCards.Event.PropertyEvents;

namespace Utils.Interfaces
{
    public interface IPropertyVisitor
    {
        void VisitCommonPropEvent(CommonPropertyEvent propertyEvent);
        void VisitPoisonEvent(PoisonEvent poisonEvent);
    }
}