namespace Model.Data.ControllersData
{
    public interface IControllerInteractionVisitor // Да усложняет код, но так правильнее с точки зрения ООП.
    {
        void Visit(IControllerInteraction interaction);
    }
}