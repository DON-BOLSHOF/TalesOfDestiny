namespace Cards.SituationCards.Event
{
    public interface ICustomButtonVisitor // Да усложняет код, но так правильнее с точки зрения ООП.
    {
        void Visit(ButtonInteraction interaction);
    }
}