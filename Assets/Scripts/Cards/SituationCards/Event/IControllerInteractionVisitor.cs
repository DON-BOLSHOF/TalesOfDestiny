using System;

namespace Cards.SituationCards.Event
{
    public interface IControllerInteractionVisitor // Да усложняет код, но так правильнее с точки зрения ООП.
    {
        void Visit(IControllerInteraction interaction);
    }

    public interface IControllerInteraction
    {
        ControllerInteractionType ControllerType { get; }
        Situation FutureSituation { get; }

        void Accept(IControllerInteractionVisitor visitor);
    }

    [Flags]
    public enum ControllerInteractionType : short
    {
        None = 0,
        Continue = 1,
        ClosePanel = 2,
        EndJourney = 4,
        Battle = 8
    }
}