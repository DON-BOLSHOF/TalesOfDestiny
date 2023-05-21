using System;
using Cards.SituationCards;

namespace Model.Data.ControllersData
{
    public interface IControllerInteraction
    {
        ControllerInteractionType ControllerType { get; }
        Situation[] ReactionSituations { get; }
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