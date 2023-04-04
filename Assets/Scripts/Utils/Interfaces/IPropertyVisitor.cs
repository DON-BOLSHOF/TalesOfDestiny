using Cards.SituationCards.Event.PropertyEvents;

namespace Utils.Interfaces
{
    public interface IPropertyVisitor
    {
        void VisitCommonPropEvent(CommonPropEvent propEvent);
        void VisitPoisonEvent(PoisonEvent poisonEvent);
    }
}