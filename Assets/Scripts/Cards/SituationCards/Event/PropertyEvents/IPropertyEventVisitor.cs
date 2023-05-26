namespace Cards.SituationCards.Event.PropertyEvents
{
    public interface IPropertyEventVisitor
    {
        void VisitCommonPropEvent(CommonPropertyEvent propertyEvent);
        void VisitPoisonEvent(PoisonEvent poisonEvent);
        void VisitBurningCampEvent(BurningCampEvent burningCampEvent);
    }
}